import {
    lstat,
    mkdir,
    readdir,
    readFile as _readFile,
    writeFile as _writeFile,
} from "fs/promises";
import { extname, join, sep, resolve } from "path";

export async function ensureDirectory(directory: string): Promise<string> {
    return mkdir(directory, { mode: 755, recursive: true }).catch((error) => {
        if (error.code !== "EEXIST") {
            throw error;
        }
        return directory;
    });
}

export async function listFiles(path: string, ext?: string): Promise<string[]> {
    const files = [];
    for (const file of await readdir(path)) {
        const filePath = join(path, file);
        const fileStat = await lstat(filePath);

        if (fileStat.isDirectory()) {
            files.push(...(await listFiles(filePath, ext)));
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

export async function writeFile(path: string, data: string): Promise<void> {
    return _writeFile(path, data, "utf-8");
}
