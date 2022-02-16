import { copyFile, lstat, mkdir, readdir, readFile as _readFile, writeFile as _writeFile, umask } from "fs/promises";
import { extname, join } from "path";

export async function copyDirectory(src: string, dest: string): Promise<void> {
    await ensureDirectory(dest);
    const files = await readdir(src);
    for (const file of files) {
        const current = await lstat(join(src, file));
        if (current.isDirectory()) {
            await copyDirectory(join(src, file), join(dest, file));
        } else {
            await copyFile(join(src, file), join(dest, file));
        }
    }
}

export async function ensureDirectory(directory: string): Promise<string | undefined> {
    const _umask = umask(0);
    return mkdir(directory, { mode: '664', recursive: true })
        .catch((error) => {
            if (error.code !== "EEXIST") {
                throw error;
            }
            return directory;
        }).finally(() => {
            umask(_umask);
        });
}

export async function listFiles(path: string, ext?: string, recursive: boolean = true): Promise<string[]> {
    const files = [];
    for (const file of await readdir(path)) {
        const filePath = join(path, file);
        const fileStat = await lstat(filePath);

        if (fileStat.isDirectory()) {
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

export async function readFile(path: string): Promise<string> {
    return _readFile(path, "utf-8");
}

export function replaceAll(input: string, find: string, replace: string): string {
    return input.replace(new RegExp(`\\${find}`, "g"), replace);
}

export async function writeFile(path: string, data: string): Promise<void> {
    console.info(`Writing ${path}...`);
    return _writeFile(path, data, "utf-8");
}
