![3licenses](https://raw.github.com/dblock/3licenses/master/3licenses.jpg)

## Motivation

The overwhelming majority of 3rd party licenses require the application that uses them to reproduce the license verbatim in an artifact that is installed with the application itself. For instance, the BSD license states the following. 

_"Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution."_

Are you currently copying individual _license.txt_ files "by hand" or are you including license text in your documentation with copy/paste? You can do much better with 3Licenses.

## 3Licenses

3Licenses is pronounced "free licenses".

* [Download 3Licenses 1.0](https://github.com/downloads/dblock/3licenses/3licenses-1.0.zip)
* [Old CodePlex Home](http://3licenses.codeplex.com/)

## Features

* Integrates with ANT and Subversion.
* Detects common license file names.
* Automatically detects various license types.
* Uses svn:externals to derive thirdparty product versions.
* Outputs thirdparty license information into XML output.
* Combines multiple thirdparty license outputs into one.
* Transforms thirdparty license information into HTML with XSLT.
* Highlights missing licenses.
* Allows overriding of product names, versions and license types.
* *Coming soon*: MSBuild Support.

## Getting Started

### Requirements

3Licenses currently works with Subversion externals only. Therefore, you must have a working Subversion repository organized with `svn:externals`. We hope that the community will contribute other implementations.

### Organizing Your Project

*Subversion Externals*

Subversion has a concept of externals. In order for 3Licenses to work you must utilize `svn:externals` that points to a respository of 3rd party libraries. 

*Organizing Thirdparty Libraries*

We recommend that you download and check-in thirdparty libraries "as-is" without any modifications into a central repository. A thirdparty SVN structure is organized by component and by version. Here's a peek into a large 3rd party structure.

![thirdparty.png](https://raw.github.com/dblock/3licenses/master/images/thirdparty.png)

This allows projects to share thirdparty libraries via `svn:externals` and allows to easily switch a project from using one version of a thirdparty library to using a newer one (by switching `svn:externals`).

*Using svn:externals*

Reference a thirdparty library by editing the `svn:externals` property of any directory. We recommend that you create a single `externals` directory at the root of your project branch in order to organize externals in a single location. This can be done with `svn propedit svn:externals <path>` or with a visual editor such as Tortoise SVN. Here's an example of an `svn:externals` of a large project.

![svnexternals.png](https://raw.github.com/dblock/3licenses/master/images/svnexternals.png)

### Collecting 3rd Party Licenses with ANT

*Reference 3Licenses JARs*

``` xml
<path id="3licenses.classpath">
 <fileset dir="${externals.dir}/3licenses">
  <include name="*.jar" />
 </fileset>
</path>
```

*Reference the 3Licenses JAR's ANT Tasks*

``` xml
 <taskdef resource="com/appsecinc/ant/3licenses.properties" classpathref="3licenses.classpath" />
```

*Create a Target to Gather Licenses*

``` xml
 <target name="gather-licenses">
  <collect-licenses src="${externals.dir}" todir="${artifacts.dir}/licenses" maxDepth="3" 
    xslfile="${externals.dir}/3licenses/manifest.xsl" />
 </target>
```

This collects licenses into `${artifacts.dir}/licenses`. You can open `manifest.xml` to see a summary. Try `ant gather-licenses`.

*Excluding Directories*

You can exclude external directories from the generated manifest, useful for excluded licenses of build tools that aren't published with the product.

``` xml
 <target name="gather-licenses">
  <collect-licenses src="${externals.dir}" todir="${artifacts.dir}/licenses" maxDepth="3" 
    xslfile="${externals.dir}/3licenses/manifest.xsl">
   <externals>
    <external src="antelope" include="false" />
    <external src="junit" include="false" />
    <external src="wix" include="false" />
   </externals>
  </collect-licenses>
 </target>
```

### Combining 3rd Party Licenses Output with ANT

*Reference 3Licenses JARs*

```
<path id="3licenses.classpath">
 <fileset dir="${externals.dir}/3licenses">
  <include name="*.jar" />
 </fileset>
</path>
```

*Reference the 3Licenses JAR's ANT Tasks*

```
 <taskdef resource="com/appsecinc/ant/3licenses.properties" classpathref="3licenses.classpath" />
```

*Create a Target to Combine Licenses*

```
 <target name="combine-licenses">
  <combine-licenses todir="${licenses.dir}/combined" xslfile="${externals.dir}/3licenses/manifest.xsl">
   <licenses srcdir="${project1.dir}/licenses" />
   <licenses srcdir="${project2.dir}/licenses" />
  </combine-licenses>
 </target>
```

Try `ant combine-licenses`. This combines multiple license outputs into `${licenses.dir}/combined`. You can open `manifest.xml` to see a summary.

### Screenshots

Some sample output from 3licenses.

*3 Licenses Thirdparty Libraries*

HTML output from 3Licenses project itself. These are the thirdparty libraries that are used by 3Licenses.

![3licenses.png](https://raw.github.com/dblock/3licenses/master/images/3licenses.png)

*XML Output*

The above example is XSL-transformed XML output.

![3licenses-xml.png](https://raw.github.com/dblock/3licenses/master/images/3licenses-xml.png)

*Combined Output*

This is partial HTML output from a large project that combines thirdparty libraries from various SVN trees.

![demo2.png](https://raw.github.com/dblock/3licenses/master/images/demo2.png)

## License

This project is licensed under the Eclipse Public License (EPL). See [LICENSE](https://raw.github.com/dblock/3licenses/master/LICENSE) for details.

## History

This project was created by and is sponsored by [Application Security, Inc.](http://www.appsecinc.com). Aside of being obligated to include 3rd party licenses in our software by the licenses themselves, we use dozens of thirdparty open-source components and are often required to fill out POC documentation that includes the list of 3rd party components with their exact versions and license. Given that we have a large project with over a dozen SVN trees (multiplied by at least 3-4 active branches), collecting thirdparty licenses became a manual nightmare. 3Licenses was born.
