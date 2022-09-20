# Argo Metaverse (open source)

## Vision

Built in Unity, the Argo Metaverse is an open-world metaverse hub for the Argo Project community.
The Argo project, in its turn, is an open source tool for Kubernetes to run workflows, manage clusters, and do GitOps right.
However, the Argo community activities encompass more than just writing software. 
With the Argo Metaverse, beyond the usual atmospheres of GitHub and websites, the Argo community engagement is taken, quite literally, to a new world!

As the first step into the development of this amazing community hub, Argo Metaverse v1.0 does not configure itself as a multi-user and fully interactive online metaverse yet.
Rather, it serves as a prototype which showcases an initial vision for the Argo Metaverse and the incredible potential it holds.

Note that the open source distribution of the project here available does not include diverse assets from third-party sources, mainly the Unity Asset Store, which fall under certain end-user agreements that prevent open distribution (see 3.3 - Plugins).
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

### Hacking backend

*****

* `pip install -r requirements/dev.txt`

Once you did it, the server will reload automagically upon your changes of the source code. You have to run tests periodically though to ensure that your changes don’t break existing functionalities. Our continuous integration server will run the whole test suite once you submit your pull-request (approx. 5 minutes).

Comments follow [Google's style guide](http://google-styleguide.googlecode.com/svn/trunk/pyguide.html#Comments).

## Versioning

Version numbering follows the [Semantic versioning](http://semver.org/) approach.

## License

We’re using open source licensing 
