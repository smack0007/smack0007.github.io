import { mkdir } from "node:fs/promises";
import { extname, join } from "node:path";
import { lstat, readdir, writeFile as nodeWriteFile } from "node:fs/promises";
import { createReadStream, createWriteStream } from "node:fs";
import { pipeline } from "node:stream/promises";
import { exec } from "node:child_process";
import { promisify } from "node:util";

export async function copyDirectory(src: string, dest: string): Promise<void> {
  await mkdir(dest, { recursive: true });
  const files = await readdir(src);
  for (const file of files) {
    const filePath = join(src, file);
    const stats = await lstat(filePath);
    if (stats.isDirectory()) {
      await copyDirectory(filePath, join(dest, file));
    } else {
      // Using streams to copy files efficiently
      const readStream = createReadStream(filePath);
      const writeStream = createWriteStream(join(dest, file));
      await pipeline(readStream, writeStream);
    }
  }
}

export async function listFiles(
  path: string,
  ext?: string,
): Promise<string[]> {
  const files: string[] = [];

  const entries = await readdir(path, { withFileTypes: true, recursive: true });
  for (const entry of entries.filter((x) => x.isFile())) {
    const filePath = join(entry.parentPath, entry.name);
    if (ext !== undefined) {
      const fileExt = extname(filePath);

      if (fileExt === ext) {
        files.push(filePath);
      }
    } else {
      files.push(filePath);
    }
  }

  return files;
}

export function replaceAll(
  input: string,
  find: string,
  replace: string,
): string {
  return input.replace(new RegExp(`\\${find}`, "g"), replace);
}

export async function runCommand(
  cwd: string,
  command: string,
  ...args: string[]
): Promise<[number, string]> {
  const execPromise = promisify(exec);

  try {
    const result = await execPromise(`${command} ${args.join(" ")}`, { cwd });
    return [0, result.stdout];
  } catch (error) {
    // @ts-ignore
    return [error.code || 1, error.stderr || error.message];
  }
}

export async function writeFile(path: string, data: string): Promise<void> {
  console.info(`Writing ${path}...`);
  return await nodeWriteFile(path, data);
}
