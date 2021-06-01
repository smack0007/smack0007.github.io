export { encode } from "html-entities";

const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
];

export function formatDate(date: Date): string {
    return `${months[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;
}

export function getTagUrl(tag: string): string {
    return `tags/${inflectFileName(tag)}/index.html`;
}

function replaceAll(input: string, find: string, replace: string): string {
    return input.replace(new RegExp(`\\${find}`, "g"), replace);
}

export function inflectFileName(input: string): string {
    input = input.toLowerCase();
    input = replaceAll(input, ".", "");
    input = replaceAll(input, "!", "");
    input = replaceAll(input, "(", "");
    input = replaceAll(input, ")", "");
    input = replaceAll(input, " ", "-");

    switch (input) {
        case "c++":
            input = "cpp";
            break;
        case "c#":
            input = "csharp";
            break;
    }

    return input;
}
