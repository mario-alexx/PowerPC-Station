using API.DTOs;
using Core.Entities;

namespace API.Extensions;

/// <summary>
/// Provides extension methods for mapping between <see cref="Address"/> and <see cref="AddressDto"/>.
/// </summary>
public static class AddressMappingExtensions
{ 
  /// <summary>
  /// Maps an <see cref="Address"/> entity to an <see cref="AddressDto"/>.
  /// </summary>
  /// <param name="address">The <see cref="Address"/> entity to be mapped.</param>
  /// <returns>An <see cref="AddressDto"/> representation of the address, or null if the address is null.</returns>
  public static AddressDto? ToDto(this Address? address) 
  {
    if(address == null) return null;

    return new AddressDto 
    {
      Line1 = address.Line1,
      Line2 = address.Line2,
      City = address.City,
      State = address.State,
      Country = address.Country,
      PostalCode = address.PostalCode,
    };
  }

  /// <summary>
  /// Maps an <see cref="AddressDto"/> to an <see cref="Address"/> entity.
  /// </summary>
  /// <param name="addressDto">The <see cref="AddressDto"/> to be mapped.</param>
  /// <returns>An <see cref="Address"/> entity based on the data from the DTO.</returns>
  public static Address ToEntity(this AddressDto addressDto) 
  {
    if(addressDto == null) throw new ArgumentNullException(nameof(addressDto));

    return new Address
    {
      Line1 = addressDto.Line1,
      Line2 = addressDto.Line2,
      City = addressDto.City,
      State = addressDto.State,
      Country = addressDto.Country,
      PostalCode = addressDto.PostalCode,
    };
  }

  /// <summary>
  /// Updates an existing <see cref="Address"/> entity from the values of an <see cref="AddressDto"/>.
  /// </summary>
  /// <param name="address">The <see cref="Address"/> entity to be updated.</param>
  /// <param name="addressDto">The <see cref="AddressDto"/> containing the updated values.</param>
  public static void UpdateFromDto(this Address address, AddressDto addressDto) 
  {
    if(addressDto == null) throw new ArgumentNullException(nameof(addressDto));
    if(address == null) throw new ArgumentNullException(nameof(address));

    address.Line1 = addressDto.Line1;
    address.Line2 = addressDto.Line2;
    address.City = addressDto.City;
    address.State = addressDto.State;
    address.Country = addressDto.Country;
    address.PostalCode = addressDto.PostalCode;
  }
}
