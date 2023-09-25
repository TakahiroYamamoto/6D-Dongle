#include <string>
#include <sstream>
#include "sntl_adminapi_cpp.h"

using namespace std;

CAdminAPIContext::CAdminAPIContext() { 
	initialize();
}

CAdminAPIContext::CAdminAPIContext(const std::string &host) {
	initialize(host);
}

CAdminAPIContext::CAdminAPIContext(const char *host) {
	initialize(host);
}

CAdminAPIContext::CAdminAPIContext(const std::string &host, sntl_admin_u16_t port) {
	initialize(host, port);
}

CAdminAPIContext::CAdminAPIContext(const char *host, sntl_admin_u16_t port) {
	initialize(host, port);
}

CAdminAPIContext::CAdminAPIContext(const std::string &host, sntl_admin_u16_t port, const std::string &pwd) {
	initialize(host, port, pwd);
}

CAdminAPIContext::CAdminAPIContext(const char *host, sntl_admin_u16_t port, const char *pwd) {
	initialize(host, port, pwd);
}

void CAdminAPIContext::initialize(const std::string &host, sntl_admin_u16_t port, const std::string &pwd) {
	this->m_vendor_code.erase();
	this->m_host = host;
	this->m_port = port;
	this->m_pwd = pwd;
	this->m_context = NULL;
}

void CAdminAPIContext::initialize(const char *host, sntl_admin_u16_t port, const char *pwd) {
	this->m_vendor_code.erase();
	if(host)
		this->m_host = host;
	if(pwd)
		this->m_pwd = pwd;
	this->m_port = port;
	this->m_context = NULL;
}

CAdminAPIContext::CAdminAPIContext(const VendorCodeType &vendorCode, const std::string &host, sntl_admin_u16_t port, const std::string &pwd) {
	if(vendorCode.getValue())
		this->m_vendor_code = vendorCode.getValue();
	this->m_host = host;
	this->m_pwd = pwd;
	this->m_port = port;
	this->m_context = NULL;
}

CAdminAPIContext::CAdminAPIContext(const VendorCodeType &vendorCode, const char *host, sntl_admin_u16_t port, const char *pwd) {
	if(vendorCode.getValue())
		this->m_vendor_code = vendorCode.getValue();
	if(host)
		this->m_host = host;
	if(pwd)
		this->m_pwd = pwd;
	this->m_port = port;
	this->m_context = NULL;
}

CAdminAPIContext::~CAdminAPIContext() {
	sntl_admin_context_delete(m_context);
}

sntl_admin_status_t  CAdminAPIContext::connect() {
	// delete previos context if any
	if (this->m_context)
		sntl_admin_context_delete(this->m_context);
	this->m_context = NULL;
	return sntl_admin_context_new_scope(&this->m_context, getScope().c_str());
}

sntl_admin_status_t CAdminAPIContext::connect(const std::string &host, sntl_admin_u16_t port, const std::string &pwd) {
	this->m_vendor_code.erase();
	this->m_host = host;
	this->m_port = port;
	this->m_pwd = pwd;
	return connect();
}

sntl_admin_status_t CAdminAPIContext::connect(const char *host, sntl_admin_u16_t port, const char *pwd) {
	this->m_vendor_code.erase();
	if(host)
		this->m_host = host;
	this->m_port = port;
	if(pwd)
		this->m_pwd = pwd;
	return connect();
}

sntl_admin_status_t CAdminAPIContext::connect(const VendorCodeType &vendorCode, const std::string &host, sntl_admin_u16_t port, const std::string &pwd) {
	if(vendorCode.getValue())
		this->m_vendor_code = vendorCode.getValue();
	this->m_host = host;
	this->m_port = port;
	this->m_pwd = pwd;
	return connect();
}

sntl_admin_status_t CAdminAPIContext::connect(const VendorCodeType &vendorCode, const char *host, sntl_admin_u16_t port, const char *pwd) {
	if(vendorCode.getValue())
		this->m_vendor_code = vendorCode.getValue();
	if(host)
		this->m_host = host;
	this->m_port = port;
	if(pwd)
		this->m_pwd = pwd;
	return connect();
}

sntl_admin_status_t CAdminAPIContext::get(const char *scope, const char *format, AdminInfo &info){
	return sntl_admin_get(m_context, scope, format, &info.m_pszInfo);
}

sntl_admin_status_t CAdminAPIContext::get(const std::string &scope, const std::string &format, std::string &info) {
	char *tmp_info = NULL;
	sntl_admin_status_t ret = sntl_admin_get(m_context, scope.c_str(), format.c_str(), &tmp_info);

	if (tmp_info != NULL)
	{
		info.assign(tmp_info);
		sntl_admin_free(tmp_info);
	}
	else
	{
		info.erase();
	}  

	return ret;
}

sntl_admin_status_t CAdminAPIContext::set(const char *input, AdminInfo &status) {
	return sntl_admin_set(m_context, input, &status.m_pszInfo);
}

sntl_admin_status_t CAdminAPIContext::set(const std::string &input, std::string &info) {
	char *tmp_info = NULL;
	sntl_admin_status_t ret = sntl_admin_set(m_context, input.c_str(), &tmp_info);
	
	if (tmp_info != NULL)
	{
		info.assign(tmp_info);
		sntl_admin_free(tmp_info);
	}
	else
	{
		info.erase();
	}  
	return ret;
}

sntl_admin_context_t* CAdminAPIContext::getContext() {
	return m_context;
}

std::string CAdminAPIContext::getScope() {
	std::string scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
	scope.append("<haspscope>");
	if(!m_vendor_code.empty()){
	scope.append("<vendor_code>" + m_vendor_code + "</vendor_code>");
	}
	scope.append("<host>" + m_host + "</host>");
	if(m_port) {
		std::ostringstream os;
		os << m_port;
		scope.append("<port>" + os.str() + "</port>");
	}
	if(!m_pwd.empty()) {
		scope.append("<password>" + m_pwd + "</password>");
	}

	scope.append("</haspscope>");

	return scope;
}
