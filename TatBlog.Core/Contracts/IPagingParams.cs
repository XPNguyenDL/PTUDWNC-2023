﻿using System.ComponentModel;

namespace TatBlog.Core.Contracts;

public interface IPagingParams
{
    int PageSize { get; set; }

    int PageNumber { get; set; }

    string SortColumn { get; set; }

    string SortOrder{ get; set; }
}