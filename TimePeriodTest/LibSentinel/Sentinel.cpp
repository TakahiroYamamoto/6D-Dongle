#include <string>
#include <atomic>
#include <memory>
#include <cstdio>
#include <vector>
#include <chrono>
#include <thread>

#ifdef NOCHECKDONGLE
bool hasp_check(std::string& errmsg)
{
	errmsg = "";
	return true;
}
#else
#include "hasp_api.h"
#include "xercesc.h"

#include "Sentinel.h"

namespace LibSentinel
{
	static const unsigned char vendor_code[] =
#ifdef __linux
		"RffDawHLNb8JftQvYcfJv3XS9HP2zigtCKP2LeVl0hFa3YB70vJopxuJickUzuQi4QCOxLnUx/hlJQnbl"
		"+I/h64Ob3eNiTpwgtHHYBcDkaQ4P8fNmoxxtZWQBXTS1i1bg+Nj3H4udgru2P04PEAOueni+GzzBrrJZ3"
		"2QZpcP9hYjBu3KNOrORxIY3O2EFYGztk9PYwzz3EJCzxRWE/aFl2MwkSAXlSLjIjMlDbDzmq404MBbIN0"
		"EQBCTUjnDFOn6hqywW7CDPhZ1vHbq2vT9PftYPfpS6831Bw0C20TzZtbw4+sG/14fDyxKv/y/dEIrkkSG"
		"+i4enryz6UtlmCTBaZolMFLN+eoPJKEkiolEqo10F0e4GbE5/cNsTprctCOElYhLL3wtlnNRTeXo/FaBF"
		"k6V1/E9P+ve3Hu3powCy+fkRAbg2hccjwbN/EOpvco/UI90pP7jqjb4sqirNdmwZDEXL/NG5JKQjYcfhk"
		"t29GRxwTs82R1lmDn5jKrP81AIz71+t1nx5HThzQBKAfbaw+Dqcybz0vRSlcU3WWwZ1AOKZuYfFQJZjsM"
		"uizjADCH9Ybfji82vj4LEjwxnm+qJN6uLly0YZNuj7Yg2hbXhfeyAkjZ3q9w7hs3zmArAQkw2u2HaPLnq"
		"2lb4Bq/vR6CkDyIBZ+vvSsP7iXxT1Cp5ZzXVr30sdSyQz4kVnRa3wt1H4NF7K/asbdWuR36WarzVHHz3E"
		"y4uEBj+PJpq27MAomPlvKHcDWtN+CXdCO7BdgdHB77I+nJ5loSeJ11j3XyqV8n/DQ9MMswoSG86zSPGvl"
		"IV11EiZzzrl1mZ43QHhUcurvCYDOiz5VUWMZx8rxKeRVgMATvpqTfrnHc8vPzLNPsoKhjFQcixlEoFUaH"
		"/BfRdUXVI0NlVAmi3v1CiyFiPoStg6rGmnEEaHc/yWe/RPnQ=";
#else
#ifdef _DEMO
		"AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsV"
		"vIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrB"
		"rh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6"
		"AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf"
		"8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1"
		"e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBh"
		"aJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SK"
		"unFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bL"
		"Cx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwL"
		"zaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9s"
		"JN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaT"
		"tLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y";
#else
		"/k0mD4BadhLfXroVy3W1JdVvHSh+J12bxl0mCM/Zr9YV8BHNULxyaTZSC01LDGKfZ+fqfMCQ4XfFi1vA"
		"wQuIIWfca7pkdV79ycUr2e+69SnuW7dfCr7aEKbuzVBYnlDHlbRgUVgtRs+gsqIXsGMW8DB89vG2gJxd"
		"M17mI3+Tbu5BnN9tNv7Z6O5rU83HmSi99ULn/ZkQmLGkdlLC8pf0Q5fo5xKtA9NgNIdNY64HwPO32fcH"
		"IMJPuPsEQr46ERTXDHoXCbiX7/CMNbQ0COwB3nIdqUyjFMJccg7JZ9NFpfAiSf0FbWskFufF8+4aAnvj"
		"R2ZMgA6Fg7JqqVLuo5Fd+f1Jtw6/5BOp2GDpNV2D+JDsJAJiShOfeNiZnsYFvMnLfaVYPF8pmZ59BVnM"
		"Vye6v/BsZe5atov/83bYfr4ke3GsrYas96CiZUE0ARIaLBrXPL/3upY2Gxtl/XC1lGm6qDt4CYZ9OW3i"
		"9QuSsql1LNCzpIjdHl6v9WrlMM0n0Jyr2EUTkCmy6MU0znDWh7aj9/LlzGt0+L80FijMP/9mRtEi0SEJ"
		"w+D2DKPR4WAZtBonXnGOQPcLFMegKZlgZpsgQyQYFfbWg18ztoUDnOtXGi7E6qkM7WKgAxi3N4yjWwil"
		"89k43J9QsxwPPDGLX2HryP0SlTLx8Elxc+n4+VNur0Z6jQ9y0D6O1CEg+T4nvuHf0pqlcFxHDnqB1bFc"
		"lyokxAU6023A0vf0Ri6p4F73McIdt2208A+0Op8uHxeXThjFVtt7JV7b7+Nm6qJ4pyBcgyaZqS9ukn7q"
		"3fQSBZXYIllsoeviXCCyWSlm4RePvTguUyFrfB6uMxrQ0PDZF4kbZt1aflIxd1SxL7CZfo4r+JrTuH/m"
		"b3B58ll4xUlqG/yjuF0IU4nYRFzLNPZjF+Nz5+JfxA1BFPTdOmByRTrQUa/+00GtEEZDm1dqIEP2Z60k"
		"frWTkec6iLfbQw5cB7mliQ==";
#endif
#endif

	static std::atomic<bool> _isAlreadyInitXercesc = false;

	bool InitXercesc(std::string& errmsg)
	{
		if (_isAlreadyInitXercesc)
			return true;
		_isAlreadyInitXercesc = true;

		try {
			XMLPlatformUtils::Initialize();
		}
		catch (const XMLException& e) {
			char* message = XMLString::transcode(e.getMessage());
			errmsg = "Error during XML parser initialization(";
			errmsg += message;
			errmsg += ")";
			XMLString::release(&message);
			return false;
		}
		return true;
	}

	class HaspPtr
	{
	public:
		char* ptr = NULL;

		~HaspPtr()
		{
			if (ptr == NULL)
				return;
			hasp_free(ptr);
		}
	};

	template <typename ... Args>
	std::string format(const std::string& fmt, Args ... args)
	{
		size_t len = std::snprintf(nullptr, 0, fmt.c_str(), args ...);
		std::vector<char> buf(len + 1);
		std::snprintf(&buf[0], len + 1, fmt.c_str(), args ...);
		return std::string(&buf[0], &buf[0] + len);
	}

	class MyDate
	{
	public:
		unsigned int year = 0;
		unsigned int month = 0;
		unsigned int day = 0;
	};

	class LicenseExpiration
	{
	public:
		int license_type = -1;
		bool is_expiration_date = false;
		hasp_time_t expiration_date;
		bool is_time_start = false;
		hasp_time_t time_start;
		bool is_total_time = false;
		hasp_time_t total_time;

		LicenseExpiration()
		{
			Init();
		}

		void Init()
		{
			license_type = -1;
			is_expiration_date = false;
			is_time_start = false;
			is_total_time = false;
		}

		bool GetExpritionDate(ExpirationInfo& exp_info)
		{
			if (license_type == 0)
			{
				exp_info.kind = 0;
			}
			else if (license_type == 1)
			{
				exp_info.kind = 1;

				unsigned int h, m, s;
				hasp_status_t status = hasp_hasptime_to_datetime(expiration_date,
					&exp_info.date.day, &exp_info.date.month, &exp_info.date.year, &h, &m, &s);
				if (status != HASP_STATUS_OK)
					return false;
				time_t now = time(NULL);
				struct tm tm_time = { 0,0,0, exp_info.date.day + 1, exp_info.date.month - 1, exp_info.date.year - 1900 };
				time_t exp = std::mktime(&tm_time);
				double remaining_sec = difftime(exp, now);
				if (remaining_sec < 0)
					exp_info.remaining_days = -1;
				else
					exp_info.remaining_days = (int)(remaining_sec / (60 * 60 * 24));
			}
			else if (license_type == 2)
			{
				exp_info.kind = 2;
				if (is_expiration_date)
				{
					unsigned int h, m, s;
					hasp_status_t status = hasp_hasptime_to_datetime(expiration_date,
						&exp_info.date.day, &exp_info.date.month, &exp_info.date.year, &h, &m, &s);
					if (status != HASP_STATUS_OK)
						return false;
					time_t now = time(NULL);
					struct tm tm_time = { 0,0,0, exp_info.date.day + 1, exp_info.date.month - 1, exp_info.date.year - 1900 };
					time_t exp = std::mktime(&tm_time);
					double remaining_sec = difftime(exp, now);
					if (remaining_sec < 0)
						exp_info.remaining_days = -1;
					else
						exp_info.remaining_days = (int)(remaining_sec / (60 * 60 * 24));
					exp_info.is_already_access = true;
				}
				else
				{
					// hasp_check時はすでにアクセスしたことになるので、このコードに来ることはないはず。
					exp_info.remaining_days = (int)(total_time / (60 * 60 * 24));
					exp_info.is_already_access = false;
				}
			}
			return true;
		}

		void ReplaceIfGood(LicenseExpiration tmp)
		{
			if (license_type == 0)
				return; // 永久ライセンスがあるなら終わり

			if (tmp.license_type == 0) // 永久ライセンスで置き換えて終わり
			{
				Init();
				license_type = 0;
				return;
			}
			if (license_type == 2 && !is_time_start) // まだ起動していないライセンスがすでにある
			{
				if (tmp.license_type == 2 && !tmp.is_time_start) // 新たなドングルでもまだ起動していないライセンスがある
				{
					if (tmp.total_time > total_time)
					{
						Init();
						license_type = 2;
						total_time = tmp.total_time;
						is_total_time = true;
					}
				}
				return; // これ以上良いライセンスはないので終わり
			}

			if (tmp.license_type == 1) // expiration date
			{
				if (tmp.is_expiration_date) // 期限日がすでに記録されている期限日より未来なら書き換える
				{
					if (!is_expiration_date || expiration_date < tmp.expiration_date)
					{
						Init();
						license_type = tmp.license_type;
						expiration_date = tmp.expiration_date;
						is_expiration_date = true;
					}
				}

			}
			else if (tmp.license_type == 2) // days_of_expiration(trial)
			{
				if (!tmp.is_total_time)
					return; // total_timeがないのは不正なので何もしない

				if (!tmp.is_time_start) // まだ起動していないライセンスならこれで書き換えて終わり。!is_time_startのチェックはここでは不要
				{
					Init();
					license_type = tmp.license_type;
					total_time = tmp.total_time;
					is_total_time = true;
					return;
				}
				else // すでに起動済みのデータ
				{
					hasp_time_t exp_date = tmp.time_start + tmp.total_time;
					if (!is_expiration_date || expiration_date < exp_date) // 既存の期限より未来置き換える
					{
						Init();
						license_type = tmp.license_type;
						expiration_date = exp_date;
						is_expiration_date = true;
						time_start = tmp.time_start;
						is_time_start = true;
						total_time = tmp.total_time;
						is_total_time = true;
					}
				}
			}
		}
	};

	void Create_Scope_String(std::vector<std::string> dongleIds, std::string& scope)
	{
		const char* scope_all_dongle =
			"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
			"<haspscope>"
			"    <hasp type=\"HASP-HL\">"
			"        <license_manager hostname=\"localhost\"/>"
			"    </hasp>"
			"</haspscope>";

		if (dongleIds.size() == 0)
		{
			scope = scope_all_dongle;
		}
		else
		{
			const char* scope_dongles_fmt =
				"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
				"<haspscope>"
				"%s" // ドングル列
				"</haspscope>";
			std::string dongles_str;
			for (int i = 0; i < dongleIds.size(); i++)
			{
				std::string id_line = format("	<hasp id=\"%s\"/>", dongleIds[i].c_str());
				dongles_str += id_line;
			}
			scope = format(scope_dongles_fmt, dongles_str.c_str());
		}
	}

	bool hasp_get_expiration_date(int feature_id, ExpirationInfo& exp_info, std::string& errmsg)
	{
		std::vector<std::string> dongleIds;
		return hasp_get_expiration_date_scope(feature_id, dongleIds, exp_info, errmsg);
	}

	bool hasp_get_expiration_date_scope_after_login(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg)
	{

		std::string scope;
		Create_Scope_String(dongleIds, scope);

		// days_of_expirationドングルに始めてアクセスした際は、hasp_get_infoで期限日をすぐに取得できない。
		// しばらく待たないと正しい情報が取得できないようだ。12秒ぐらいかかるようなので諦めた
		int i;
		for (i = 0; i < 1; i++)
		{
			if (!hasp_get_expiration_date_scope(feature_id, dongleIds, exp_info, errmsg))
				return false;
			if (exp_info.kind != 2)
				break;
			if (exp_info.is_already_access)
				break;
			if (exp_info.date.year != 0)
				break;
			/* これをやってもあまり意味がない感じ
			hasp_status_t   status;
			hasp_handle_t   handle;
			hasp_login_scope(feature_id, scope.c_str(), vendor_code, &handle);
			hasp_logout(handle);
			*/
			// std::this_thread::sleep_for(std::chrono::milliseconds(1000));
		}
		return true;
	}

	bool hasp_get_expiration_date_scope(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg)
	{
		std::string xml_errmsg;

		// ライセンス期限を調べる

		std::string scope;
		Create_Scope_String(dongleIds, scope);

		const char* info_str =
			"<haspformat root=\"hasp_info\">"
			"<feature>"
			"<attribute name=\"id\"/>"
			"<element name=\"license\"/>"
			"<hasp>"
			"<attribute name=\"id\"/>"
			"<attribute name = \"type\"/>"
			"</hasp>"
			"</feature>"
			"</haspformat>";


		std::string xmlInfo;
		{
			HaspPtr info;
			hasp_status_t status = hasp_get_info(scope.c_str(), info_str, vendor_code, &info.ptr);
			if (status != HASP_STATUS_OK)
			{
				errmsg = format("Cannot get license info.[%d]", status);
				return false;
			}
			xmlInfo = info.ptr;
		}

		LicenseExpiration licExpiration;

		try {
			XMLPlatformUtils::Initialize();
		}
		catch (const XMLException& e) {
			char* message = XMLString::transcode(e.getMessage());
			errmsg = "License error: XML parser initialization(";
			errmsg += message;
			errmsg += ")";
			XMLString::release(&message);
			return false;
		}
		XercesDOMParser* parser = new XercesDOMParser();
		parser->setValidationScheme(XercesDOMParser::Val_Always); // optional
		parser->setDoNamespaces(true); // optional

		ErrorHandler* errHandler = (ErrorHandler*) new HandlerBase();
		parser->setErrorHandler(errHandler);

		bool isError = false;
		do
		{
			try {
				const char* info = xmlInfo.c_str();
#ifdef _DEBUG
				{
					int debug = 0;
					if (debug > 0)
					{
						FILE* fp;
						fopen_s(&fp, "D:\\tmp\\licinfo.txt", "w");
						fprintf(fp, "%s", info);
						fclose(fp);
					}

				}
#endif
				MemBufInputSource myxml_buf((const XMLByte*)info, xmlInfo.length(), "myxml (in memory)");
				parser->parse(myxml_buf);

				DOMDocument* doc = parser->getDocument();

				std::string node_tag_str;
				std::string node_value_str;
				std::string attr_name_str;
				std::string attr_value_str;
				// std::string content_str;
				for (DOMNode* top_node = doc->getFirstChild(); top_node; top_node = top_node->getNextSibling())
				{
					if (licExpiration.license_type == 0)
						break;

					if (top_node->getNodeType() != DOMNode::ELEMENT_NODE)
						continue;

					DOMElement* elem = static_cast<DOMElement*>(top_node);
					char* node_tag = XMLString::transcode(elem->getTagName());
					node_tag_str = node_tag;
					XMLString::release(&node_tag);
					if (node_tag_str == "hasp_info")
					{
						for (DOMNode* info_node = top_node->getFirstChild(); info_node; info_node = info_node->getNextSibling())
						{
							if (licExpiration.license_type == 0)
								break;

							if (info_node->getNodeType() != DOMNode::ELEMENT_NODE)
								continue;

							DOMElement* elem = static_cast<DOMElement*>(info_node);
							char* node_tag = XMLString::transcode(elem->getTagName());
							node_tag_str = node_tag;
							XMLString::release(&node_tag);
							if (node_tag_str != "feature")
								continue;

							DOMNamedNodeMap* map = elem->getAttributes();
							bool isTargetFeature = false;
							for (int i_attr = 0; i_attr < map->getLength(); i_attr++)
							{
								DOMAttr* attr = static_cast<DOMAttr*>(map->item(i_attr));
								char* attr_name = XMLString::transcode(attr->getName());
								char* attr_value = XMLString::transcode(attr->getValue());
								attr_name_str = attr_name;
								attr_value_str = attr_value;
								XMLString::release(&attr_name);
								XMLString::release(&attr_value);
								if (attr_name_str == "id")
								{
									int fid = atoi(attr_value_str.c_str());
									if (fid == feature_id)
										isTargetFeature = true;
									break;
								}
							}
							if (!isTargetFeature)
								continue;

							for (DOMNode* feature_node = info_node->getFirstChild(); feature_node; feature_node = feature_node->getNextSibling())
							{
								if (licExpiration.license_type == 0)
									break;

								if (feature_node->getNodeType() != DOMNode::ELEMENT_NODE)
									continue;

								DOMElement* elem = static_cast<DOMElement*>(feature_node);
								char* node_tag = XMLString::transcode(elem->getTagName());
								node_tag_str = node_tag;
								XMLString::release(&node_tag);
								if (node_tag_str != "license")
									continue;

								LicenseExpiration tmp_licExpiration;
								for (DOMNode* lic_node = feature_node->getFirstChild(); lic_node; lic_node = lic_node->getNextSibling())
								{
									if (licExpiration.license_type == 0)
										break;

									if (lic_node->getNodeType() != DOMNode::ELEMENT_NODE)
										continue;

									DOMElement* elem = static_cast<DOMElement*>(lic_node);
									char* node_tag = XMLString::transcode(elem->getTagName());
									node_tag_str = node_tag;
									XMLString::release(&node_tag);
									{
										DOMNode* lic_child_node = lic_node->getFirstChild();
										DOMElement* elem = static_cast<DOMElement*>(lic_child_node);
										char* node_value = XMLString::transcode(elem->getTextContent());
										node_value_str = node_value;
										XMLString::release(&node_value);
									}

									if (node_tag_str == "license_type")
									{
										if (node_value_str == "perpetual")
											tmp_licExpiration.license_type = 0;
										else if (node_value_str == "expiration")
											tmp_licExpiration.license_type = 1;
										else  if (node_value_str == "trial")
											tmp_licExpiration.license_type = 2;
									}
									else if (node_tag_str == "exp_date")
									{
										tmp_licExpiration.expiration_date = std::stoull(node_value_str);
										tmp_licExpiration.is_expiration_date = true;
									}
									else if (node_tag_str == "time_start")
									{
										try
										{
											tmp_licExpiration.time_start = std::stoull(node_value_str);
											tmp_licExpiration.is_time_start = true;
										}
										catch (...)
										{
											tmp_licExpiration.is_time_start = false;
										}
									}
									else if (node_tag_str == "total_time")
									{
										tmp_licExpiration.total_time = std::stoull(node_value_str);
										tmp_licExpiration.is_total_time = true;
									}
								}
								licExpiration.ReplaceIfGood(tmp_licExpiration);
							}
						}
					}
				}
			}
			catch (const XMLException& e) {
				char* message = XMLString::transcode(e.getMessage());
				errmsg = format("Cannot get license info:XML error.[%s]", message);
				XMLString::release(&message);
				isError = true;
			}
			catch (const DOMException& e) {
				char* message = XMLString::transcode(e.msg);
				errmsg = format("Cannot get license info:XML error.[%s]", message);
				XMLString::release(&message);
				isError = true;
			}
			catch (...) {
				errmsg = format("Cannot get license info:XML error.[unknown]");
				isError = true;
			}
		} while (0);

		delete parser;
		delete errHandler;

		XMLPlatformUtils::Terminate();

		if (isError)
			return false;

#if true
		std::string date_str;
		if (!licExpiration.GetExpritionDate(exp_info))
		{
			errmsg = format("Cannot get license expiration info.");
			return false;
		}
#else
		if (!licExpiration.GetDateString(year, month, day, remaining_days)) // 期限情報文字列を得る
		{
			errmsg = format("Cannot get license expiration info.");
			return false;
		}
		if (isExpired)
		{
			errmsg = format("License error: expired[%s]", date_str.c_str());
			return false;
		}
#endif
		return true;
	}
	bool hasp_check(int feature_id,  ExpirationInfo& exp_info, std::string& errmsg)
	{
		std::vector<std::string> scope;
		return hasp_check_scope(feature_id, scope, exp_info, errmsg);
	}
	bool hasp_check_scope(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg)
	{
		hasp_status_t   status;
		hasp_handle_t   handle;

		std::string scope;
		if (dongleIds.size() > 0)
		{
			scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
			scope += "<haspscope>";
			for (std::string id_str : dongleIds)
			{
				std::string id_line = format("	<hasp id=\"%s\"/>", id_str.c_str());
				scope += id_line;
			}
			scope += "</haspscope>";
			status = hasp_login_scope(feature_id, scope.c_str(),
				(hasp_vendor_code_t)vendor_code,
				&handle);
		}
		else
		{
			status = hasp_login(feature_id,
				(hasp_vendor_code_t)vendor_code,
				&handle);
		}
		hasp_logout(handle);

		bool isExpired = false;

		errmsg = "";
		switch (status)
		{
		case HASP_STATUS_OK:
			break;

		case HASP_FEATURE_NOT_FOUND:
			errmsg = "License error:0 Feature not found";
			break;

		case HASP_CONTAINER_NOT_FOUND:
			errmsg = "License error:1 Container not found";
			break;

		case HASP_OLD_DRIVER:
			errmsg = "License error:2 Old driver";
			break;

		case HASP_NO_DRIVER:
			errmsg = "License error:3 No driver";
			break;

		case HASP_INV_VCODE:
			errmsg = "License error:4 Inv_VCODE";
			break;
		case HASP_FEATURE_EXPIRED:
			isExpired = true;
			break;
		default:
			char buf[256];
#ifdef __linux
			sprintf(buf, "License error:5 dcode=%d", status);
#else
			sprintf_s(buf, "License error:5 dcode=%d", status);
#endif
			errmsg = buf;
		}
		if (errmsg == "" || isExpired)
		{
			if (!hasp_get_expiration_date_scope_after_login(feature_id, dongleIds, exp_info, errmsg))
				return false;
			if (isExpired)
			{
				errmsg = format("License error: expired[on %04d-%02d-%02d]", 
					exp_info.date.year, exp_info.date.month, exp_info.date.day);
				return false;
			}
		}
		else
			return false;

		return true;
	}
}
#endif	/* NOCHECKDONGLE */
