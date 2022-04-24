# Play LLC

## Folder Description

>This folder contains custom objects and mappings to enhance the capability of the nuget package
'AutoFaq'. Autofaq simplifies setup of concrete objects when creating unit tests, much like the 
Moq framework does for interfaces.
 

## Subfolders

> ### SpecimenBuilders
>>The SpecimenBuilders folder contains custom objects that instruct AutoFaq how to initialize
an object. When an object's constructor has validation logic, or restricts specific ranges,
AutoFixture often throws an exception because it doesn't know how to handle the business logic.
These ISpecimenBuilders solve that problem and allow us to easily create those objects in our
unit tests without worrying about an exception being thrown

 