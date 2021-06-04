import * as process from "process";
import { readFile } from "fs/promises";
import { join } from "path";
import { inflectFileName } from "~/compiler";
import { ensureDirectory, writeFile } from "~/compiler/utils";

const ROOT_DIRECTORY = join(__dirname, "..");
const OUTPUT_DIRECTORY = join(ROOT_DIRECTORY, "source", "blog");

main().then(() => process.exit(0));

async function main() {
    if (process.argv[2] === undefined || process.argv[2] === "") {
        console.error("Please provide a name for the new post.");
        process.exit(1);
    }

    const title = process.argv[2];
    const date = new Date(Date.now());

    let template = await readFile(join(__dirname, "new-post.md"), "utf-8");

    template = template
        .replace("{{title}}", title)
        .replace(
            "{{date}}",
            `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, "0")}-${date
                .getDate()
                .toString()
                .padStart(2, "0")}`
        );

    const fileName = join(`${inflectFileName(title)}.md`);

    await ensureDirectory(join(OUTPUT_DIRECTORY, date.getFullYear().toString()));
    await writeFile(join(OUTPUT_DIRECTORY, date.getFullYear().toString(), fileName), template);
}
