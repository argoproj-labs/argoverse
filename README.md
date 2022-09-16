# Argo Metaverse (open source)

## Vision

Built in Unity, the Argo Metaverse is an open-world metaverse hub for the Argo Project community.
The Argo project, in its turn, is an open source tool for Kubernetes to run workflows, manage clusters, and do GitOps right.
However, the Argo community activities encompass more than just writing software. 
With the Argo Metaverse, beyond the usual atmospheres of GitHub and websites, the Argo community engagement is taken, quite literally, to a new world!

As the first step into the development of this amazing community hub, Argo Metaverse v1.0 does not configure itself as a multi-user and fully interactive online metaverse yet.
Differently, it serves as a prototype which showcases an initial vision for the Argo Metaverse and the incredible potential it holds.

Moreover, the open source distribution of the project here available does not include diverse assets from third-party sources, mainly the Unity Asset Store, which fall under certain end-user agreements that prevent open distribution (see 3.3 - Plugins).
To abide by such terms, these assets have been removed from the open source version of the project, and different scenes were created to facilitate development without them. (See Documentation for more information)


## History

We started this project in mid-June, 2022 for internal use.

We open-sourced it on September 16, 2022.


## Installation

We’re using these technologies: Unity version 2021.3.4f1 LTS
(see Documentation for information on other requirements)

The actual Unity project folder is: argo-metaverse/ArgoMetaverse_opensource

Once the editor above is installed and the repository is cloned to your machine, you have to "Open" the project ("Add project from disk)" through Unity Hub selecting the ArgoMetaverse_opensource folder inside the root repository folder.



Enjoy!

## Contributing

We’re really happy to accept contributions from the community, that’s the main reason why we open-sourced it! There are many ways to contribute, even if you’re not a technical person.

We’re using the infamous [simplified Github workflow](http://scottchacon.com/2011/08/31/github-flow.html) to accept modifications (even internally), basically you’ll have to:

* create an issue related to the problem you want to fix (good for traceability and cross-reference)
* fork the repository
* create a branch (optionally with the reference to the issue in the name)
* hack hack hack
* commit incrementally with readable and detailed commit messages
* submit a pull-request against the master branch of this repository

We’ll take care of tagging your issue with the appropriated labels and answer within a week (hopefully less!) to the problem you encounter.

If you’re not familiar with open-source workflows or our set of technologies, do not hesitate to ask for help! We can mentor you or propose good first bugs (as labeled in our issues). Also welcome to add your name to Credits section of this document.

### Submitting bugs

You can report issues directly on Github, that would be a really useful contribution on testing on the project. Please document as much as possible the steps to reproduce your problem (even better with screenshots).

### Discussing strategies

We’re trying to develop this project in the open as much as possible. We have a dedicated mailing-list where we discuss each new strategic change and invite the community to give a valuable feedback. You’re encouraged to subscribe to it and participate.

### Adding documentation

We’re doing our best to document each usage of the project but you can improve it or add you own sections. The documentation is available within the /docs/ folder.

### Improving User eXperience

*****

### Hacking backend

*****

* `pip install -r requirements/dev.txt`

Once you did it, the server will reload automagically upon your changes of the source code. You have to run tests periodically though to ensure that your changes don’t break existing functionalities. Our continuous integration server will run the whole test suite once you submit your pull-request (approx. 5 minutes).

Commit messages should be formatted using [AngularJS conventions](http://goo.gl/QpbS7) (one-liners are OK for now but body and footer may be required as the project matures).

Comments follow [Google's style guide](http://google-styleguide.googlecode.com/svn/trunk/pyguide.html#Comments).


## Financing

Please contact us. 

## Versioning

Version numbering follows the [Semantic versioning](http://semver.org/) approach.

## License

We’re using open source licensing 

## Rationale for forking (optional)

We forked the full Argo Metaverse project (GitLab) because:

* it had to be turned into an open source project
* it should be available through GitHub


## Credits

* [Intuit](https://www.intuit.com/)
* [Mile 80](https://www.mile80.com/)
