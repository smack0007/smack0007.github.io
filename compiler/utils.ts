import { ensureDir } from "std/fs/mod.ts";
import { extname, join } from "std/path/mod.ts";

export async function copyDirectory(src: string, dest: string): Promise<void> {
  await ensureDir(dest);
  const files = await Deno.readDir(src);
  for await (const file of files) {
    const current = await Deno.lstat(join(src, file.name));
    if (current.isDirectory) {
      await copyDirectory(join(src, file.name), join(dest, file.name));
    } else {
      await Deno.copyFile(join(src, file.name), join(dest, file.name));
    }
  }
}

export async function listFiles(
  path: string,
  ext?: string,
  recursive = true
): Promise<string[]> {
  const files: string[] = [];
  for await (const file of await Deno.readDir(path)) {
    const filePath = join(path, file.name);
    const fileStat = await Deno.lstat(filePath);

    if (fileStat.isDirectory) {
      if (recursive) {
        files.push(...(await listFiles(filePath, ext)));
      }
    } else {
      if (ext !== undefined) {
        const fileExt = extname(filePath);

        if (fileExt === ext) {
          files.push(filePath);
        }
      } else {
        files.push(filePath);
      }
    }
  }

  return files;
}

export function replaceAll(
  input: string,
  find: string,
  replace: string
): string {
  return input.replace(new RegExp(`\\${find}`, "g"), replace);
}

export async function runCommand(
  cwd: string,
  command: string,
  ...args: string[]
): Promise<[number, string]> {
  const process = new Deno.Command(command, {
    cwd,
    args,
    stdout: "piped",
  });

  const { code, stdout } = await process.output();

  return [code, new TextDecoder().decode(stdout)];
}

export async function writeFile(path: string, data: string): Promise<void> {
  console.info(`Writing ${path}...`);
  return await Deno.writeTextFile(path, data);
}
