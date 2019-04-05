import React from 'react'
import { Container } from 'reactstrap'
import Helmet from 'react-helmet'
import Layout from '../components/layout'
import styles from './conference.module.css'

export default function Template ({ pageContext }) {
  const c = pageContext.conference
  return (
    <Layout>
      <div>
        <Helmet title={`${c.title}`} />
        <Container>
          <h3>{c.title}</h3>
          <a href={c.url}>{c.url}</a>
          <div>
            <span>{c.location}</span>
            <span className={styles.date}>{c.startDate}</span>
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
        </Container>
      </div>
    </Layout>
  )
}
