#pragma once
#include <stdio.h>
#include <xercesc/util/XercesDefs.hpp>
#include <xercesc/parsers/XercesDOMParser.hpp>
#include <xercesc/dom/DOM.hpp>
#include <xercesc/dom/DOMNode.hpp>
#include <xercesc/util/XMLString.hpp>
#include <xercesc/util/PlatformUtils.hpp>
#include <xercesc/sax/HandlerBase.hpp>
#include <xercesc/framework/LocalFileFormatTarget.hpp>
#include <xercesc/framework/MemBufInputSource.hpp>

// #include "DOMPrintFilter.hpp"

XERCES_CPP_NAMESPACE_USE

/** XMLCh to char **/
class Char2X {
    XMLCh* fUnicodeForm;
public:
    Char2X(const char* const toTranscode) { fUnicodeForm = XMLString::transcode(toTranscode); }
    ~Char2X() { delete[] fUnicodeForm; }
    const XMLCh* unicodeForm() const { return fUnicodeForm; }
};

/** char to XMLCh **/
class X2Char {
    char* fcharForm;
public:
    X2Char(const XMLCh* const toTranscode) { fcharForm = XMLString::transcode(toTranscode); }
    ~X2Char() { delete[] fcharForm; }
    const char* charForm() const { return fcharForm; }
};

#define C2X(str) Char2X(str).unicodeForm()
#define X2C(str) X2Char(str).charForm()
//-----------------------------------------------------------------
class SimpleErrorHandler : public DOMErrorHandler
{
public:
    bool handleError(const DOMError& domError)
    {
        printf("%s, line %s, char %s: %s\n",
            X2C(domError.getLocation()->getURI()),
            (char*)domError.getLocation()->getLineNumber(),
            (char*)domError.getLocation()->getColumnNumber(),
            X2C(domError.getMessage()));
        return domError.getSeverity() != DOMError::DOM_SEVERITY_FATAL_ERROR;
    }
};
//-----------------------------------------------------------------
