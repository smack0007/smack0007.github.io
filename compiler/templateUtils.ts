import { Page } from "./types";

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
    return `${
        months[date.getMonth()]
    } ${date.getDate()}, ${date.getFullYear()}`;
}

export function isActiveUrl(url: string, page: Page): boolean {
    return url.endsWith(page.url);
}
