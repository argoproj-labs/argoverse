# ArgoVerse

## Vision

The ArgoVerse is a social space for the Argo Community, driven by GitOps!
* Celebrate shared milestones and experiences
* Promote diversity and inclusion

The ArgoVerse is an experiment in immersive virtual user experiences for Argo.
* Shared persistent digital workspaces
* Frictionless engagement and collaboration between teams and users

The initial implementation of the ArgoVerse as not a multi-user environemnt.
Rather, it serves as a prototype which showcases an initial vision for the ArgoVerse.

Note that the open source distribution of the project here available does not include diverse assets from third-party sources, mainly the Unity Asset Store, which fall under certain end-user agreements that prevent open distribution (see 3.3 - Plugins).
To abide by such terms, these assets have been removed from the open source version of the project, and different scenes were created to facilitate development without them. (See Documentation for more information)

## Installation

We’re using these technologies: Unity version 2021.3.4f1 LTS
(see Documentation for information on other requirements)

The actual Unity project folder is: argoverse/ArgoMetaverse_opensource

Once the editor above is installed and the repository is cloned to your machine, you have to "Open" the project ("Add project from disk)" through Unity Hub selecting the ArgoMetaverse_opensource folder inside the root repository folder.

### Hacking backend

*****

* `pip install -r requirements/dev.txt`

Once you did it, the server will reload automagically upon your changes of the source code. You have to run tests periodically though to ensure that your changes don’t break existing functionalities. Our continuous integration server will run the whole test suite once you submit your pull-request (approx. 5 minutes).

Comments follow [Google's style guide](http://google-styleguide.googlecode.com/svn/trunk/pyguide.html#Comments).

## Versioning

Version numbering follows the [Semantic versioning](http://semver.org/) approach.
