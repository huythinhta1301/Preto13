﻿using Preto13.Model;

namespace Preto13.Data.Common
{
    public interface iStatus
    {
        Task<GenericResponse> SHOW_ALL_STATUS();
        Task<GenericResponse> INSERT_NEW_STATUS(string VN_NAME, string EN_NAME, string DESCRIPTION);
    }
}
