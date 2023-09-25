#include <string>
#include "sntl_adminapi_cpp.h"


////////////////////////////////////////////////////////////////////
// Construction/Destruction
////////////////////////////////////////////////////////////////////

AdminInfo::AdminInfo()
    : m_pszInfo(NULL)
{
}

AdminInfo::~AdminInfo()
{
    clear();
}

////////////////////////////////////////////////////////////////////
// Implementation
////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////
//
////////////////////////////////////////////////////////////////////
AdminInfo::operator const char *() const
{
	return m_pszInfo;
}

////////////////////////////////////////////////////////////////////
//
////////////////////////////////////////////////////////////////////
void AdminInfo::clear()
{
    if (NULL != m_pszInfo)
    {
        sntl_admin_free(m_pszInfo);
        m_pszInfo = NULL;
    }
}

////////////////////////////////////////////////////////////////////
//
////////////////////////////////////////////////////////////////////
const char* AdminInfo::getInfo() const
{
	return m_pszInfo ? m_pszInfo : "";
}