using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;

public class EditAddressHandler
	: IRequestHandler<EditAddressRequest, Guid>
{
	private readonly IRepo<AddressBookEntry> _repo;

	public EditAddressHandler(IRepo<AddressBookEntry> repo)
	{
		_repo = repo;
	}

	// Handles the update request
	public async Task<Guid> Handle(EditAddressRequest request, CancellationToken cancellationToken)
	{
		// Find the entry matching the id of the request
		var entries = _repo.Find(new EntryByIdSpecification(request.Id));
		
		// If there are no entries or multiple entries- throw an error
		if (entries.Count == 0)
			throw new KeyNotFoundException("Error updating entry: Entry doesn't exist! It was likely deleted while you were editing.");
		else if(entries.Count > 1)
			throw new InvalidOperationException("Error updating entry: Multiple duplicate entries with same GUID. Something has gone VERY wrong with the database.");

		// Update the entry
		entries[0].Update(
			request.Line1,
			request.Line2,
			request.City,
			request.State,
			request.PostalCode
		);
		
		// Updating the repo isn't necessary, since the object has
		// already been updated. This makes the _repo.Update function
		// essentially useless- I'm honestly not sure what best practice is in
		// this case.
		//_repo.Update(entries[0]);

		// Return witht the GUID of the request
		return await Task.FromResult(request.Id);
	}
}