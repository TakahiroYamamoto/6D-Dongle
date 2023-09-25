#if !defined(__SNTL_ADMINAPI_CPP_H__)
#define __SNTL_ADMINAPI_CPP_H__

#if !defined(SNTLCPP_DECL)
	#define SNTLCPP_DECL
#endif // SNTLCPP_DECL

#if defined(WIN32) || defined(WIN64)
	#define SNTL_TARGET_WINDOWS
#endif // WIN32 || WIN64

////////////////////////////////////////////////////////////////////
// Windows
////////////////////////////////////////////////////////////////////
#if defined(SNTL_TARGET_WINDOWS)

// exclude rarely used window stuff
#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#endif // SNTL_TARGET_WINDOWS

#if !defined(__SNTL_ADMINAPI_H__)
extern "C" {
#include "sntl_adminapi.h"
}
#endif // __SNTL_ADMINAPI_H__

#include <string>

////////////////////////////////////////////////////////////////////
// struct AdminInfo
////////////////////////////////////////////////////////////////////
struct AdminInfo
{
// Construction/Destruction
public:
	AdminInfo();
	~AdminInfo();

// Attributes
public:
	char* m_pszInfo;

// Operators
public:
	operator const char*() const;

// Implementation
public:
	void clear();
	const char* getInfo() const;
};

////////////////////////////////////////////////////////////////////
// struct VendorCodeType
////////////////////////////////////////////////////////////////////
struct VendorCodeType
{
// Construction/Destruction
public:
	VendorCodeType(const std::string &vendorCode) : m_vendorCodeStr(vendorCode) {
	};

	VendorCodeType(const char* vendorCode) : m_vendorCodeStr(vendorCode) {
	};

private:
	VendorCodeType();
	VendorCodeType(const VendorCodeType &vendorCodeType);

// Attributes
private:
	std::string m_vendorCodeStr;

// Implementation
public:
	const char* getValue() const{
		return m_vendorCodeStr.c_str();
	};
};

class SNTLCPP_DECL CAdminAPIContext
{

	public:
		CAdminAPIContext();

		CAdminAPIContext(const std::string &host);

		CAdminAPIContext(const char *host);

		CAdminAPIContext(const std::string &host, sntl_admin_u16_t port);

		CAdminAPIContext(const char *host, sntl_admin_u16_t port);

		CAdminAPIContext(const std::string &host, sntl_admin_u16_t port, const std::string &pwd);

		CAdminAPIContext(const char *host, sntl_admin_u16_t port, const char *pwd);

		CAdminAPIContext(const VendorCodeType &vendorCode, const std::string &host, sntl_admin_u16_t port = 0, const std::string &pwd = std::string(""));
		CAdminAPIContext(const VendorCodeType &vendorCode, const char *host, sntl_admin_u16_t port = 0, const char *pwd = NULL);

		~CAdminAPIContext();

		sntl_admin_status_t connect();

		sntl_admin_status_t connect(const char *host, sntl_admin_u16_t port, const char *pwd);
		sntl_admin_status_t connect(const std::string &host, sntl_admin_u16_t port, const std::string &pwd);

		sntl_admin_status_t connect(const VendorCodeType &vendorCode, const std::string &host, sntl_admin_u16_t port, const std::string &pwd);
		sntl_admin_status_t connect(const VendorCodeType &vendorCode, const char *host, sntl_admin_u16_t port, const char *pwd);

		sntl_admin_status_t get(const char *scope, const char *format, AdminInfo &info);
		sntl_admin_status_t get(const std::string  &scope, const std::string &format, std::string &info);

		sntl_admin_status_t set(const char *input, AdminInfo &status);
		sntl_admin_status_t set(const std::string  &input, std::string &status);

		sntl_admin_context_t * getContext();

	private:
		CAdminAPIContext(const CAdminAPIContext &cadminapicontext);
		CAdminAPIContext & operator= (const CAdminAPIContext &other);

		void initialize(const std::string &host = std::string(""), sntl_admin_u16_t port = 0, const std::string &pwd = std::string(""));

		void initialize(const char *host, sntl_admin_u16_t port = 0, const char *pwd = NULL);

		std::string getScope();

		std::string m_host;
		sntl_admin_u16_t m_port;
		std::string m_pwd;
		std::string m_vendor_code;
		sntl_admin_context_t *m_context;

};


#endif // __SNTL_ADMINAPI_CPP_H__