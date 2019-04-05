import React from 'react'
import { Container, Row } from 'reactstrap'
import Link from 'gatsby-link'
import { graphql } from 'gatsby'
import Layout from '../components/layout'
import styles from './index.module.css'

const IndexPage = ({ data }) => {
  const posts = data.allMarkdownRemark.edges.filter(
    post =>
      !post.node.frontmatter.hidden &&
      post.node.frontmatter.contentType === 'blog'
  )

  const conferences = data.postgres.allConferences.edges.map(n => n.node)

  return (
    <Layout>
      <Container>
        {posts.map(({ node: post }) => (
          <div key={post.id}>
            <h2>
              <Link to={post.frontmatter.path}>{post.frontmatter.title}</Link>
            </h2>
            <p style={{ marginBottom: 10 }}>{post.frontmatter.date}</p>
            <p>{post.excerpt}</p>
          </div>
        ))}
      </Container>
      <Container className={styles.conferenceList}>
        <Row>
          {conferences.map(c => (
            <div className='col-md-6' key={c.slug}>
              <div className={styles.conference}>
                <h4>
                  <Link to={`/conference/${c.slug}`}>{c.title}</Link>
                </h4>
                <div>
                  <span>{c.location}</span>
                  <span className={styles.right}>{c.startDate}</span>
                </div>
                <div hidden={!c.cfpDeadline}>
                  <span hidden={!c.cfpLink}>
                    <a href={c.cfpLink}>CFP Open Till {c.cfpDeadline}</a>
                  </span>
                  <span hidden={c.cfpLink}>CFP Open Till {c.cfpDeadline}</span>
                </div>
                <div hidden={!c.twitter}>
                  <a href={`https://twitter.com/${c.twitter}`}>{c.twitter}</a>
                </div>
              </div>
            </div>
          ))}
        </Row>
      </Container>
    </Layout>
  )
}

export default IndexPage

export const pageQuery = graphql`
  query {
    allMarkdownRemark(sort: { order: DESC, fields: [frontmatter___date] }) {
      edges {
        node {
          excerpt(pruneLength: 400)
          id
          frontmatter {
            title
            contentType
            date(formatString: "MMMM DD, YYYY")
            path
            hidden
          }
        }
      }
    }
    postgres {
      allConferences(orderBy: STARTDATE_ASC, condition: { status: "ACTIVE" }) {
        edges {
          node {
            title
            slug
            location
            startDate: startdate
            twitter
            cfpDeadline: cfpdeadline
            cfpLink: cfplink
          }
        }
      }
    }
  }
`
