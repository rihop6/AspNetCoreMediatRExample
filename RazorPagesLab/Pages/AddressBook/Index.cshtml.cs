using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class IndexModel : PageModel
{
	private readonly IRepo<AddressBookEntry> _repo;
	public IEnumerable<AddressBookEntry> AddressBookEntries;

	public IndexModel(IRepo<AddressBookEntry> repo)
	{
		_repo = repo;
	}

	public void OnGet()
	{
		AddressBookEntries = _repo.Find(new AllEntriesSpecification());
	}

	// Method for posting the delete from the modal
	public IActionResult OnPostDelete(Guid id)
	{
		var entries = _repo.Find(new EntryByIdSpecification(id));
		if (entries.Count == 1)
			_repo.Remove(entries[0]);
			
		return RedirectToPage();
	}
}