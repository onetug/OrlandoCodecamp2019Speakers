const connectionString = process.env.CONFERENCES_DB_CONNECTIONSTRING
const disqusKey = process.env.CONFERENCES_DISQUS_KEY || 'gatsby-starter-blog'

module.exports = {
  siteMetadata: {
    title: 'findTech.events',
    author: 'Coding Blocks',
    authorLink: 'https://github.com/codingblocks',
    disqus: disqusKey
  },
  plugins: [
    // Source Plugins
    {
      resolve: 'gatsby-source-filesystem',
      name: 'Basic Pages',
      options: {
        path: `${__dirname}/src/pages`,
        name: 'pages'
      }
    },
    {
      resolve: 'gatsby-source-filesystem',
      name: 'Examples',
      options: {
        path: `${__dirname}/src/examples`,
        name: 'examples'
      }
    },
    {
      resolve: 'gatsby-source-pg',
      options: {
        connectionString: connectionString,
        schema: 'public'
      }
    },

    // Transform Plugins
    {
      resolve: 'gatsby-transformer-remark',
      options: {
        plugins: ['gatsby-remark-prismjs', 'gatsby-remark-copy-linked-files']
      }
    },
    'gatsby-transformer-json',

    // Miscellaneous plugins
    'gatsby-plugin-offline',
    'gatsby-plugin-react-helmet',
    {
      resolve: 'gatsby-plugin-sass',
      options: {
        includePaths: [`${__dirname}/node_modules`, `${__dirname}/src/`],
        precision: 8
      }
    }
  ]
}
