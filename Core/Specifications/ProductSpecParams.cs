using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Specifications
{
  public class ProductSpecParams
  {
    private const int MaxPageSize = 50;
    private int _pageIndex = 1;

    public int PageIndex { get => _pageIndex; set => _pageIndex = value; }

    private int _pageSize = 6;
    public int PageSize
    {
      get { return _pageSize; }
      set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
    }

    private int? _brandId;
    public int? BrandId
    {
      get { return _brandId; }
      set { _brandId = value; }
    }

    private int? _typeId;
    public int? TypeId
    {
      get { return _typeId; }
      set { _typeId = value; }
    }
    public string? Sort { get; set; }

    private string? _search = string.Empty;
    public string? Search
    {
      get { return _search; }
      set { _search = value?.ToLower(); }
    }

  }
}