[![Netlify Status](https://api.netlify.com/api/v1/badges/72731c13-3b8e-4ea4-87a1-5b53c1ac05ed/deploy-status)](https://app.netlify.com/sites/condescending-gates-bec8ba/deploys)

Check out the video about this project:
https://www.youtube.com/watch?v=Z2JK7SS82wE

# Intro to JAMstack

JAMstack is a set of best practices for building decoupled front-ends with a heavy emphasis on build-time rendering.

It’s quickly gaining popularity because the managed services and front-end tooling have reached a tipping point where it’s now easy to make great static sites at build time.

Because of this, the responsibilities of Front-End and Back-End developers are changing. Check out the slides to see where I think things are going.

- [YouTube video of the presentation](https://www.youtube.com/watch?v=Z2JK7SS82wE)
- [Download the presentation](https://github.com/codingblocks/intro-to-jamstack/blob/master/jamstack.pptx)
- Demo site: [findTech.events](https://findTech.events)
- [Orlando Code Camp Session](https://www.orlandocodecamp.com/Sessions/Details/76)

## Working with the code

This site requires a connection to postgres sql, and the schema has not been checked in anywhere yet. Shoot us a message if you are interested in actually running this code and we will help you set things up!

### Environment Settings

- CONFERENCES_DB_CONNECTIONSTRING (required)
- CONFERENCES_DISQUS_KEY (optional)

Running locally

```
gatsby develop
```

Publishing

```
gatsby build
```

## Resources we like:

[JAMstack.org](https://jamstack.org/)
Home page for JAMstack, tons of [https://jamstack.org/resources/](https://jamstack.org/resources/)

[StaticGen](https://www.staticgen.com/)
Great collection of static site generators. Browse my technologies and license.

[JAMstack Radio](https://www.heavybit.com/library/podcasts/jamstack-radio/)
Podcast focused around JAMstack

[Coding Blocks on JAMstack](https://www.codingblocks.net/podcast/jamstack-with-j-a-m/)
Great Podcast _wink_ feature discussion and debate over what JAMstack is, and what it means for the future
