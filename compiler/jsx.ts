declare global {
    namespace JSX {
        interface Element extends String {}
        interface IntrinsicElements {
            [key: string]: any;
        }
    }
}

type HTMLAttributes = { [key: string]: any };

const NO_CLOSING_TAG = ["link", "meta"];

export function html(
    tagName: string,
    attributes: HTMLAttributes | null,
    ...children: unknown[]
): string {
    let output = `<${tagName}`;

    if (attributes !== null) {
        for (const [key, value] of Object.entries(attributes)) {
            output += ` ${key}="${value}"`;
        }
    }

    output += ">";

    for (const child of children) {
        if (child !== false) {
            output += child;
        }
    }

    if (!NO_CLOSING_TAG.includes(tagName)) {
        output += `</${tagName}>`;
    }

    return output;
}
