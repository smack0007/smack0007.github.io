import { html, Page } from "~/compiler";

export function PageTemplate(page: Page) {
    return (
        "<!doctype html>" +
        (
            <html lang="en">
                <head>
                    <meta charset="utf-8"></meta>
                    <meta
                        name="viewport"
                        content="width=device-width, initial-scale=1.0, maximum-scale=1"
                    ></meta>
                    <meta name="description" content=""></meta>
                    <meta name="author" content="Zachary Snow"></meta>

                    <title>{page.frontMatter.title}</title>
                    <base href="/"></base>

                    <link
                        href="https://fonts.googleapis.com/css?family=Open+Sans"
                        rel="stylesheet"
                    ></link>
                    <link
                        rel="stylesheet"
                        type="text/css"
                        href="css/style.css"
                    ></link>

                    <link
                        rel="alternate"
                        type="application/rss+xml"
                        title="The Blog of Zachary Snow"
                        href="feed.rss"
                    ></link>

                    <link
                        rel="apple-touch-icon"
                        sizes="180x180"
                        href="/apple-touch-icon.png"
                    ></link>
                    <link
                        rel="icon"
                        type="image/png"
                        sizes="32x32"
                        href="/favicon-32x32.png"
                    ></link>
                    <link
                        rel="icon"
                        type="image/png"
                        sizes="16x16"
                        href="/favicon-16x16.png"
                    ></link>
                    <link rel="manifest" href="/site.webmanifest"></link>
                </head>
                <body>
                    <div class="wrap">
                        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                            <h1>
                                <a class="navbar-brand" href="/">
                                    The Blog of Zachary Snow
                                </a>
                            </h1>
                            <button
                                id="navbar-toggler"
                                class="navbar-toggler"
                                type="button"
                                aria-controls="navbarSupportedContent"
                                aria-expanded="false"
                                aria-label="Toggle navigation"
                            >
                                <span class="navbar-toggler-icon"></span>
                            </button>

                            <div
                                class="collapse navbar-collapse"
                                id="navbarSupportedContent"
                            >
                                <ul class="navbar-nav mr-auto">
                                    <li class="nav-item">
                                        <a class="nav-link" href="index.html">
                                            Home
                                            {page.url.endsWith(
                                                "index.html"
                                            ) && (
                                                <span class="sr-only">
                                                    (current)
                                                </span>
                                            )}
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="about.html">
                                            About
                                            {page.url.endsWith(
                                                "about.html"
                                            ) && (
                                                <span class="sr-only">
                                                    (current)
                                                </span>
                                            )}
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" href="tags.html">
                                            Tags
                                            {page.url.endsWith("tags.html") && (
                                                <span class="sr-only">
                                                    (current)
                                                </span>
                                            )}
                                        </a>
                                    </li>
                                </ul>
                                <div class="social my-2 my-lg-0">
                                    <a
                                        href="https://twitter.com/smack0007"
                                        class="twitter"
                                        title="Twitter"
                                    >
                                        <span class="icon-twitter"></span>
                                    </a>
                                    <a
                                        href="https://github.com/smack0007"
                                        class="github"
                                        title="Github"
                                    >
                                        <span class="icon-github"></span>
                                    </a>
                                    <a
                                        href="https://paypal.me/smack0007"
                                        class="coffee"
                                        title="Buy me a Coffee"
                                    >
                                        <span class="icon-mug"></span>
                                    </a>
                                    <a href="feed.rss" class="rss" title="RSS">
                                        <span class="icon-rss"></span>
                                    </a>
                                </div>
                            </div>
                        </nav>
                        <main class="container">
                            <div class="posts">{page.contents}</div>
                            <div class="clear"></div>

                            {/* @if (ShowPagination)
                {
                    <nav aria-label="Page navigation">
                        <ul class="pagination justify-content-center">
                            <li class="page-item @(PaginationOlderLink == null ? "disabled" : "")">
                                @if (PaginationOlderLink != null)
                                {
                                    <a href="@(PaginationOlderLink)" class="page-link older">Older</a>
                                }
                                @if (PaginationOlderLink == null)
                                {                    
                                    <a class="page-link older" href="#" tabindex="-1">Older</a>
                                }
                            </li>
                            <li class="page-item @(PaginationNewerLink == null ? "disabled" : "")">
                                @if (PaginationNewerLink != null)
                                {
                                    <a href="@(PaginationNewerLink)" class="page-link newer">Newer</a>                    
                                }
                                @if (PaginationNewerLink == null)
                                {
                                    <a class="page-link newer" href="#" tabindex="-1">Newer</a>
                                }
                            </li>              
                        </ul>
                    </nav>
                } */}
                        </main>
                        <footer class="p-13 p-md-5 mt-5 text-center text-muted bg-light">
                            <div class="container">
                                <ul class="links">
                                    <li>
                                        <a
                                            href="https://twitter.com/smack0007"
                                            class="twitter"
                                            title="Twitter"
                                        >
                                            Twitter
                                        </a>
                                    </li>
                                    <li>
                                        <a
                                            href="https://github.com/smack0007"
                                            class="github"
                                            title="Github"
                                        >
                                            GitHub
                                        </a>
                                    </li>
                                    <li>
                                        <a
                                            href="https://paypal.me/smack0007"
                                            class="coffee"
                                            title="Buy me a Coffee"
                                        >
                                            Buy me a Coffee
                                        </a>
                                    </li>
                                    <li>
                                        <a
                                            href="feed.rss"
                                            class="rss"
                                            title="RSS"
                                        >
                                            RSS
                                        </a>
                                    </li>
                                </ul>
                                <p class="mb-0">The Blog of Zachary Snow</p>
                            </div>
                        </footer>
                    </div>
                    {/* <script type="text/javascript">
            document.getElementById('navbar-toggler').onclick = function() {
                document.getElementById('navbarSupportedContent').classList.toggle('collapse');
            };
        </script> */}
                </body>
            </html>
        )
    );
}
