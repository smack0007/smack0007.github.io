// Bootstrapper for ts-node
import { Module } from "module";
import { readFileSync } from "fs";
import { join, resolve } from "path";

const tscConfig = JSON.parse(
    readFileSync(join(__dirname, "tsconfig.json"), "utf8")
);
const paths: { [key: string]: string } = {};

for (const key of Object.keys(tscConfig["compilerOptions"]["paths"])) {
    const pathTo = tscConfig["compilerOptions"]["paths"][key][0];
    paths[key.substring(0, key.length - 1)] = pathTo.substring(
        0,
        pathTo.length - 1
    );
}

const defaultResolveFilename = (Module as any)._resolveFilename;
(Module as any)._resolveFilename = function (
    request: string,
    parent: any
): string {
    for (const key of Object.keys(paths)) {
        if (request.startsWith(key)) {
            request = join(
                __dirname,
                request.replace(key, paths[key] as string)
            );
        }
    }

    return defaultResolveFilename(request, parent);
};
