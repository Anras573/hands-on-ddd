# Chapter 06: Acting With Commands

## Notes:

Naming of folders in the Marketplace project.
 - Api contains the controllers
 - Contracts contains the models -> Alexey writes that the "contract" models are practically DTO's or POCO's.

He plans to use several versions in the same file. 

He talks about the Command Handler pattern, but decide against using it in this book.

The Controller is so small a minimal API might make more sense.

In Docker Compose he's using latest, which is not recommended. - It makes more sense to use a specific version (preferably the same as the one used in Production).
