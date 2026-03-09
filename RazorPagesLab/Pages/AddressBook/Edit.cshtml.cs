using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class EditModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

	public EditModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	[BindProperty]
	public EditAddressRequest EditAddressRequest { get; set; }

	// Run on getting this page
	public void OnGet(Guid id)
	{
		// Get the entries with the same GUID
		IReadOnlyList<AddressBookEntry> entries = _repo.Find(new EntryByIdSpecification(id));
		// Check the number of entries- anything other than 1 is an error
		switch(entries.Count)
		{
			// If there is only 1 entry, it was retrieved successfully. Set the values in the model.
			case 1:
				AddressBookEntry entry = entries[0];
				EditAddressRequest = new EditAddressRequest
				{
					Id = entry.Id,
					Line1 = entry.Line1,
					Line2 = entry.Line2,
					City = entry.City,
					State = entry.State,
					PostalCode = entry.PostalCode
				};
				break;
			// If there are no entries, it was likely deleted right as edit was requested. Send error message.
			case 0:
				ModelState.AddModelError("Entry Error", "Error fetching entry: Entry doesn't exist! This likely happened because it was deleted as you accessed it.");
				break;
			// If there are more than 1 entry, there are duplicate GUIDs in the database. Send error message.
			default:
				ModelState.AddModelError("Entry Error", "Error fetching entry: Multiple duplicate entries with same GUID. Something has gone VERY wrong with the database.");
				break;
		}
	}

	// Runs on the 'Update Address' button press
	public async Task<ActionResult> OnPost()
	{
		// If the model state is valid...
		if (ModelState.IsValid)
			// Try to send the update through the mediator and return to index
			try
			{
				_ = await _mediator.Send(EditAddressRequest);
				return RedirectToPage("Index");
			}
			// If there was an error, add it to the Entry Error in the model.
			catch (Exception ex)
			{
				ModelState.AddModelError("Entry Error", ex.Message);
			}

		return Page();
	}
}