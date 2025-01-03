/* eslint-disable @typescript-eslint/no-explicit-any */
declare module "markdown-it" {
    interface Options {
      html?: boolean;
      xhtmlOut?: boolean;
      breaks?: boolean;
      langPrefix?: string;
      linkify?: boolean;
      typographer?: boolean;
      quotes?: string;
      highlight?: (str: string, lang: string) => string;
    }
  
    interface MarkdownIt {
      render: (md: string, env?: any) => string;
      renderInline: (md: string, env?: any) => string;
      use: (plugin: any, ...options: any[]) => MarkdownIt;
    }
  
    const MarkdownIt: new (options?: Options) => MarkdownIt;
    export default MarkdownIt;
  }
  