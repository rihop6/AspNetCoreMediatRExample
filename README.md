# Razor Pages & MediatR Lab Exercise
This is the solution for the Razor Pages & MediatR Lab Exercise. Below is a summary of the implementation:
## 1. Edit Address (update)
Functionality for editing the database was fully implemented.
### Edit.cshtml.cs
This is essentially the controller for the html page. OnGet and OnPost were implemented.
- OnGet populates the fields of the form by initializing EditAddressRequest with the values of the entry that is being edited when the page is fetched
- OnPost uses the mediator to update the repository if the model state is correct when the form is submitted

### EditAddressHandler.cs
This is the request handler that acts as the 'bridge' between the repository and the backend when making requests. This primarily uses the 'Handle' function. The 'Handle' function takes the following steps:
- Fetch the entry from the database using the EditAddressRequest given
- Error handling for cases where there are no entries or duplicate entries
- Update the entry with the values in the EditAddress Request
- Return result as the request id

<small>Note: the _repo.Update function is not actually accessed here, since updating the entry itself will update the object in the _repo. With a different _repo implementation that is decoupled from the entry object, _repo.Update may need to be run.</small>

### EditAddressRequest.cs
This essentially defines the 'schema' for relating the data in the form to the entry in repository. It also provides form validation.
- Renamed from UpdateAddressRequest to EditAddressRequest
- Added 'Entry Error' field for handling misc errors like database errors

<small>note: I feel I made a slight mistake calling this 'Edit' instead of 'Update'. To keep consistent with the concept of 'CRUD' operations, 'Update' makes more sense. However, its a tiny mistake. If this were a real project, I'd fix it.</small>

## 2. Delete
Functionality for deleting entries from the database was added. This uses a modal on the main page to confirm if the user wants to delete the entry.

### Index.cshtml
This is the main page showing all the entries. Each entry has an 'edit' and 'delete' button. Clicking the 'delete' button opens a modal that can be used to delete the entry.
- Changed the 'delete' button from a link to a button
- Clicking the button toggles a modal, and data regarding the entry is sent to the modal
- In the modal, clicking the delete button will call the OnPostDelete function in Index.cshtml.cs

<small>note: admittedly much of the modal code was copy/pasted from another one of my projects. My frontend brain took the wheel for awhile.</small>

### Index.cshtml.cs
This is the controller for the main page. It has a new function OnPostDelete that is triggered when the modal's delete button is pressed. This method does the following:
- Find entry
- Remove entry
- Refresh page