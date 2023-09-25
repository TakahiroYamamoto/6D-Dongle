{ *****************************************************************************
  *
  * Sentinel LDK Licensing API DEMO program for the update process
  *
  * Copyright (C) 2016, SafeNet, Inc. All rights reserved.
  *
  ***************************************************************************** }

{$APPTYPE CONSOLE}
{$I+}                  { enable default i/o error handling }

uses
  System.sysutils,
  hasp_unit;

{$I hasp_vcode.inc }   { this header file holds the Sentinel DEMOMA vendor code }

var
  buffer: pointer;
  cml_switch_0: string;
  status: hasp_status_t;
  action: AnsiString;
  scope: AnsiString;
  info: PAnsiChar;
  recipient: PAnsiChar;
  h2r: PAnsiChar;
  v2c: PAnsiChar;
  fh: TextFile;

procedure usage;
begin
  writeln('Usage:     hasp_update <option> [filename]');
  writeln;
  writeln('Options:   u: updates a Sentinel protection key/attaches a detached license');
  writeln('           i: retrieves Sentinel protection key information');
  writeln('           d: detaches a license from Sentinel SL key');
  writeln('           r: rehost a license from a Sentinel SL-AdminMode/SL-UserMode key');
  writeln;
  writeln('Filename:  Path to the V2C/R2H file (in case of update/attach),');
  writeln('              OR');
  writeln('           Path to the C2V/H2R file (uses stdout if filename is not specified)');
  writeln;
  halt(1);
end;

procedure read_file(file_name: String; var buffer: pointer);
var
  file_handle: File of byte;
  read_size: integer;
  buf_size: integer;
begin
  assignfile(file_handle, file_name);
  reset(file_handle);

  buf_size := FileSize(file_handle) + 1; { add room for trailing zero }
  getmem(buffer, buf_size);
  fillchar(buffer^, buf_size, #0);
  BlockRead(file_handle, buffer^, FileSize(file_handle), read_size);
  CloseFile(file_handle);
end;

begin

  writeln;
  writeln('This is a simple demo program for the Sentinel LDK update functions.');
  writeln('Copyright (C) SafeNet, Inc. All rights reserved.');
  writeln;

  if (ParamCount = 0) then
    usage;

  cml_switch_0 := AnsiLowerCase(ParamStr(1));

  if (cml_switch_0 = 'd') then
  begin
    scope := '<haspscope>'#13#10 +
      '  <license_manager hostname="localhost" />'#13#10 + '</haspscope>'#13#10;
    writeln(scope);
    writeln(HASP_RECIPIENT);

    { get local recipient information }
    status := hasp_get_info(PAnsiChar(scope), HASP_RECIPIENT,
      PAnsiChar(vendor_code), info);

    if (status <> HASP_STATUS_OK) then
    begin

      case status of

        HASP_SCOPE_RESULTS_EMPTY:
          writeln('Unable to locate a feature matching the scope.');

        HASP_INSUF_MEM:
          writeln('Out of memory.');

        HASP_INV_VCODE:
          writeln('Invalid vendor code.');

        HASP_UNKNOWN_VCODE:
          writeln('Unknown vendor code.');

        HASP_INVALID_PARAMETER:
          writeln('Invalid parameter (scope/format too long?).');

        HASP_DEVICE_ERR:
          writeln('Input/Output error.');

        HASP_LOCAL_COMM_ERR:
          writeln('Communication error.');

        HASP_REMOTE_COMM_ERR:
          writeln('Remote communication error.');

        HASP_TOO_MANY_KEYS:
          writeln('Too many keys connected.');

        HASP_INV_FORMAT:
          writeln('Unrecognized format string');

        HASP_INV_SCOPE:
          writeln('Unrecognized scope string.');

      else
        writeln('hasp_get_info failed with status ', status);

      end;

    end;

    recipient := info;

    { detach license for local recipient (duration 120 seconds.) }
    action := '<?xml version="1.0" encoding="UTF-8" ?>' +
      '<detach><duration>120</duration></detach>';
    scope := '<haspscope><product id="123" /></haspscope>';
    status := hasp_transfer(PAnsiChar(action), PAnsiChar(scope),
      PAnsiChar(vendor_code), recipient, h2r);

    if (status <> HASP_STATUS_OK) then
    begin

      case status of

        HASP_INV_DETACH_ACTION:
          writeln('Invalid XML detach_action parameter.');

        HASP_INV_RECIPIENT:
          writeln('Invalid XML recipient parameter.');

        HASP_TOO_MANY_PRODUCTS:
          writeln('Scope for hasp_detach does not select a unique product.');

        HASP_ACCESS_DENIED:
          writeln('Request denied because of LMS access restrictions.');

        HASP_INV_PRODUCT:
          writeln('Invalid product information.');

        HASP_INSUF_MEM:
          writeln('Out of memory');

        HASP_DEVICE_ERR:
          writeln('Input/Output error.');

        HASP_LOCAL_COMM_ERR:
          writeln('Communication error.');

        HASP_REMOTE_COMM_ERR:
          writeln('Remote communication error.');

        HASP_INV_SCOPE:
          writeln('Unrecognized scope string');

      else
        writeln('hasp_transfer failed with status ', status);

      end;

    end;

    if (ParamCount = 2) then
    begin
      assignfile(fh, ParamStr(2));
      ReWrite(fh);
      writeln(fh, h2r);
      CloseFile(fh);
    end
    else
    begin
      writeln(h2r);
    end;

  end
  else

    if (cml_switch_0 = 'r') then
  begin
    scope := '<haspscope>'#13#10 +
      '  <license_manager hostname="localhost" />'#13#10 + 
      '</haspscope>'#13#10;
    writeln(scope);
    writeln(HASP_RECIPIENT);

    { get local recipient information }
    status := hasp_get_info(PAnsiChar(scope), HASP_RECIPIENT,
      PAnsiChar(vendor_code), info);

    if (status <> HASP_STATUS_OK) then
    begin

      case status of

        HASP_SCOPE_RESULTS_EMPTY:
          writeln('Unable to locate a feature matching the scope.');

        HASP_INSUF_MEM:
          writeln('Out of memory.');

        HASP_INV_VCODE:
          writeln('Invalid vendor code.');

        HASP_UNKNOWN_VCODE:
          writeln('Unknown vendor code.');

        HASP_INVALID_PARAMETER:
          writeln('Invalid parameter (scope/format too long?).');

        HASP_DEVICE_ERR:
          writeln('Input/Output error.');

        HASP_LOCAL_COMM_ERR:
          writeln('Communication error.');

        HASP_REMOTE_COMM_ERR:
          writeln('Remote communication error.');

        HASP_TOO_MANY_KEYS:
          writeln('Too many keys connected.');

        HASP_INV_FORMAT:
          writeln('Unrecognized format string');

        HASP_INV_SCOPE:
          writeln('Unrecognized scope string.');

      else
        writeln('hasp_get_info failed with status ', status);

      end;

    end;

    recipient := info;

    { hasp_transfer
      This function is used to rehost the v2c form one machine
      to another machine. For this we will use the recipient
      information generated from hasp_get_info function. }

    action := '<?xml version="1.0" encoding="UTF-8" ?>' +
      '<rehost><hasp id="123456789"/></rehost>';
    scope := '<haspscope><hasp id="123456789"/></haspscope>';
    status := hasp_transfer(PAnsiChar(action), PAnsiChar(scope),
      PAnsiChar(vendor_code), recipient, v2c);

    if (status <> HASP_STATUS_OK) then
    begin

      case status of

        HASP_INV_DETACH_ACTION:
          writeln('Invalid XML detach_action parameter.');

        HASP_INV_RECIPIENT:
          writeln('Invalid XML recipient parameter.');

        HASP_TOO_MANY_PRODUCTS:
          writeln('Scope for hasp_detach does not select a unique product.');

        HASP_ACCESS_DENIED:
          writeln('Request denied because of LMS access restrictions.');

        HASP_INV_PRODUCT:
          writeln('Invalid product information.');

        HASP_INSUF_MEM:
          writeln('Out of memory');

        HASP_DEVICE_ERR:
          begin
            writeln('Input/Output error.');
          end;

        HASP_LOCAL_COMM_ERR:
          begin
            writeln('Communication error.');
          end;

        HASP_REMOTE_COMM_ERR:
          begin
            writeln('Remote communication error.');
          end;

        HASP_INV_SCOPE:
          begin
            writeln('Unrecognized scope string');
          end;

      else
        begin
          writeln('hasp_transfer failed with status ', status);
        end;

      end;

    end;

    if (ParamCount = 2) then
    begin
      assignfile(fh, ParamStr(2));
      ReWrite(fh);
      writeln(fh, v2c);
      CloseFile(fh);
    end
    else
    begin
      writeln(v2c);
    end;

  end
  else

    if (cml_switch_0 = 'u') then
  begin
    if (ParamCount = 2) then
    begin
      read_file(ParamStr(2), buffer);
      status := hasp_update(buffer, info);

      if (status <> HASP_STATUS_OK) then
      begin

        case status of

          HASP_INV_UPDATE_DATA:
            writeln('Update data is invalid');

          HASP_INV_UPDATE_NOTSUPP:
            writeln('Update data is invalid');

          HASP_INV_UPDATE_CNTR:
            writeln('Update counter not set correctly');

          HASP_INSUF_MEM:
            writeln('Out of memory');

          HASP_DEVICE_ERR:
            writeln('Input/Output error');

          HASP_LOCAL_COMM_ERR:
            writeln('Communication error');

          HASP_NO_ACK_SPACE:
            writeln('Acknowledge pointer is NULL');

          HASP_UNKNOWN_ALG:
            writeln('Unknown v2c algorithm');

          HASP_INV_SIG:
            writeln('v2c signature broken');

          HASP_TOO_MANY_KEYS:
            writeln('Too many keys connected');

          HASP_HARDWARE_MODIFIED:
            writeln('v2c data and SL key data mismatch');

          HASP_UPDATE_TOO_OLD:
            writeln('v2c was already installed');

          HASP_UPDATE_TOO_NEW:
            writeln('v2c is too new (update counter too high)');

        else
          writeln('hasp_update failed with status', status);

        end;

      end;

      FreeMem(buffer);

    end

    else

    begin
      writeln('invalid parameter count !');
    end;

  end

  else

    if (cml_switch_0 = 'i') then
  begin

    { restrict the c2v to local Sentinel keys }
    scope := '<haspscope>'#13#10 +
      '  <license_manager hostname="localhost" />'#13#10 + 
      '</haspscope>';
    status := hasp_get_info(PAnsiChar(scope), HASP_UPDATEINFO,
      PAnsiChar(vendor_code), info);

    if (status <> HASP_STATUS_OK) then
    begin

      case status of

        HASP_SCOPE_RESULTS_EMPTY:
          writeln('Unable to locate a Feature matching the scope');

        HASP_INSUF_MEM:
          writeln('Out of memory');

        HASP_INV_VCODE:
          writeln('Invalid vendor code');

        HASP_UNKNOWN_VCODE:
          writeln('Unknown vendor code');

        HASP_INVALID_PARAMETER:
          writeln('Invalid parameter (scope/format too long?)');

        HASP_DEVICE_ERR:
          writeln('Input/Output error');

        HASP_LOCAL_COMM_ERR:
          writeln('Communication error');

        HASP_REMOTE_COMM_ERR:
          writeln('Remote communication error');

        HASP_TOO_MANY_KEYS:
          writeln('Too many keys connected');

        HASP_INV_FORMAT:
          writeln('Unrecognized format string');

        HASP_INV_SCOPE:
          writeln('Unrecognized scope string');

      else
        writeln('hasp_get_info failed with status ', status);

      end;

    end;

    if (ParamCount = 2) then
    begin
      assignfile(fh, ParamStr(2));
      ReWrite(fh);
      writeln(fh, info);
      CloseFile(fh);
    end
    else
    begin
      writeln(info);
    end;

  end

  else

  begin
    writeln('switch not supported !');
    writeln;
    usage();
  end;

end.
