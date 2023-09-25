{ *****************************************************************************
  *
  * Sentinel LDK Licensing API DEMO program
  *
  * Copyright (C) 2016, SafeNet, Inc. All rights reserved.
  *
  * Sentinel LDK DEMOMA key required with feature id 1 and 42 enabled
  *
  ***************************************************************************** }

{$APPTYPE CONSOLE}
{$I+}                          { enable default i/o error handling }

uses
  hasp_unit,
  System.SysUtils;

{$I hasp_vcode.inc }           { this header file holds the Sentinel DEMOMA vendor code }

const
  AES_MIN_DATA_LEN = 16; { the minimum data length for AES encryption/decryption }
  MAX_MEMORY = 128; { the maximum memory size to be handled here }
  CUSTOM_FEATURE = 42;
  DEMO_MEMBUFFER_SIZE = 128;

var
  data: array [0 .. 15] of byte = (
    $74,
    $65,
    $73,
    $74,
    $20,
    $73,
    $74,
    $72,
    $69,
    $6E,
    $67,
    $20,
    $31,
    $32,
    $33,
    $34
  );
  membuffer: array [0 .. DEMO_MEMBUFFER_SIZE - 1] of byte;
  datalen: hasp_size_t;

  { helper functions:
    hexbyte: convert a byte into 2 digit string
    dump   : dumps a given block of data, in hex and ascii
  }

function hexbyte(b: byte): AnsiString;
const
  hexchar: AnsiString = '0123456789ABCDEF';
begin
  result := hexchar[(b shr 4) + 1] + hexchar[(b and 15) + 1];
end;

procedure dump(var data; datalen: integer; margin: AnsiString);
var
  i, j: integer;
  b: AnsiChar;
  s: AnsiString;
  mydata: array [0 .. 1000] of AnsiChar absolute data;

begin
  if (datalen = 0) then
    exit;

  s := '';
  j := 0;
  for i := 0 to datalen - 1 do
  begin
    if (j = 0) then
      write(margin);
    b := mydata[i];
    if ((b < #32) or (b > #127)) then
      s := s + '.'
    else
      s := s + b;
    if (j = 7) then
      s := s + ' ';
    write(hexbyte(byte(b)), ' ');
    inc(j);
    if (((j and 3) = 0) and (j < 15)) then
      write('| ');
    if (j > 15) then
    begin
      writeln('  ', s);
      s := '';
      j := 0;
    end;
  end;

  if (j > 0) then
  begin
    while (j < 16) do
    begin
      write('   ');
      inc(j);
      if (((j and 3) = 0) and (j < 15)) then
        write('| ');
    end;
    writeln('  ', s);
  end;
end;

procedure encrypt_decrypt(handle: hasp_handle_t);
var
  status: hasp_status_t;

begin

  { hasp_encrypt encrypts a block of data using the Sentinel key
    (minimum buffer size is 16 bytes).
  }
  datalen := sizeof(data);
  dump(data, datalen, '    ');

  status := hasp_encrypt(handle, data, datalen);

  case status of

    HASP_STATUS_OK:
      begin
        writeln('encryption ok:');
        dump(data, datalen, '    ');
      end;

    HASP_INV_HND:
      writeln('handle not active'#13#10);

    HASP_TOO_SHORT:
      writeln('data length too short');

    HASP_ENC_NOT_SUPP:
      writeln('attached key does not support AES encryption');

    HASP_CONTAINER_NOT_FOUND:
      writeln('key/license container not available');

  else
    writeln('encryption failed');

  end;
  if status <> 0 then
    Halt(1);

  { hasp_decrypt decrypts a block of data using the Sentinel key
    (minimum buffer size is 16 bytes).
  }
  status := hasp_decrypt(handle, data, datalen);

  case status of

    HASP_STATUS_OK:
      begin
        writeln('decryption ok:');
        dump(data, datalen, '    ');
      end;

    HASP_INV_HND:
      writeln('handle not active');

    HASP_TOO_SHORT:
      writeln('data length too short');

    HASP_ENC_NOT_SUPP:
      writeln('attached key does not support AES encryption');

  else
    writeln('decryption failed');

  end;
  if status <> 0 then
    Halt(1);
end;

var
  status: hasp_status_t;
  handle: hasp_handle_t;
  handle2: hasp_handle_t;
  fsize: hasp_size_t;
  time: hasp_time_t;
  info: PAnsiChar;
  i: integer;
  day, month, year, hour, minute, second: cardinal;
  scope: AnsiString;
  view: AnsiString;
  major_ver, min_ver, build_serv, build_num: integer;

begin
  writeln;
  writeln('A simple demo program for the Sentinel LDK licensing functions');
  writeln('Copyright (C) SafeNet, Inc. All rights reserved.');
  writeln;

  hasp_get_version(major_ver, min_ver, build_serv, build_num,
    PAnsiChar(vendor_code));
  writeln('version = ' + IntToStr(major_ver) + ':' + IntToStr(min_ver) + ':' +
    IntToStr(build_serv) + ':' + IntToStr(build_num));
  writeln;

  {
    ******************************************************
    * hasp_login
    *   establishes a context for Sentinel services.
    *
    *   login to default feature (0). This default feature
    *   is available on any key search for local and
    *   remote Sentinel keys
    ******************************************************
  }
  write('login to default feature         : ');
  status := hasp_login(HASP_DEFAULT_FID, PAnsiChar(vendor_code), handle);

  case status of

    HASP_STATUS_OK:
      writeln('OK');

    HASP_FEATURE_NOT_FOUND:
      writeln('login to default feature failed');

    HASP_CONTAINER_NOT_FOUND:
      writeln('no Sentinel key with vendor code DEMOMA found');

    HASP_OLD_DRIVER:
      writeln('outdated driver version installed');

    HASP_NO_DRIVER:
      writeln('Sentinel driver not installed');

    HASP_INV_VCODE:
      writeln('invalid vendor code');

  else
    writeln('login to default feature failed with status ', status);

  end;
  if status <> 0 then
    Halt(1);

  {
    ******************************************************
    * hasp_get_sessioninfo
    *   retrieve Sentinel key attributes
    *
    * Please note:
    *   In case of performing an activation we recommend
    *   to use hasp_get_info() instead of
    *   hasp_get_sessioninfo(), as demonstrated in the
    *   activation sample. hasp_get_info() can be called
    *   without performing a login.
    ******************************************************
  }

  write(#13#10'get session info                 : ');
  status := hasp_get_sessioninfo(handle, HASP_KEYINFO, info);

  case status of

    HASP_STATUS_OK:
      begin
        writeln('OK, Sentinel key attributes retrieved'#13#10);
        writeln('Key info:'#13#10'==============='#13#10 + info +
          '===============');
        {
          *****************************************************************
          * hasp_free
          *   frees memory allocated by hasp_get_info, hasp_get_sessioninfo
          *   or hasp_update (if an acknowledge was requested)
          *****************************************************************
        }
        hasp_free(info);
      end;

    HASP_INV_HND:
      writeln('handle not active');

    HASP_INV_FORMAT:
      writeln('unrecognized format');

    HASP_CONTAINER_NOT_FOUND:
      writeln('key/license container not available');

  else
    writeln('hasp_get_sessioninfo failed');

  end;
  if status <> 0 then
    Halt(1);

  {
    ******************************************************
    * hasp_get_size retrieve the memory size of the
    * Sentinel key.
    ******************************************************
  }

  write('retrieving the key''s memory size : ');
  status := hasp_get_size(handle, HASP_FILEID_RW, fsize);

  case status of

    HASP_STATUS_OK:
      writeln(fsize, ' bytes');

    HASP_INV_HND:
      writeln('handle not active');

    HASP_INV_FILEID:
      writeln('invalid file id');

    HASP_CONTAINER_NOT_FOUND:
      writeln('key/license container not available');

  else
    writeln('could not retrieve memory size');

  end;
  if status <> 0 then
    Halt(1);

  { skip memory access if no memory available }
  if fsize <> 0 then
  begin
    {
      ******************************************************
      * hasp_read
      *   read from Sentinel key memory.
      *
      *   limit memory size to be used in this demo program.
      ******************************************************
    }
    if fsize > MAX_MEMORY then
    begin
      fsize := MAX_MEMORY;
    end;

    write(#13#10'reading ', fsize:4, ' bytes from memory   : ');
    status := hasp_read(handle, HASP_FILEID_RW, 0, fsize, membuffer);

    case status of

      HASP_STATUS_OK:
        begin
          writeln('OK');
          dump(membuffer, fsize, '    ');
        end;

      HASP_INV_HND:
        writeln('handle not active');

      HASP_INV_FILEID:
        writeln('invalid file id');

      HASP_MEM_RANGE:
        writeln('beyond memory range of attached Sentinel key');

      HASP_CONTAINER_NOT_FOUND:
        writeln('key/license container not available');

    else
      writeln('read memory failed');

    end;
    if status <> 0 then
      Halt(1);

    {
      ********************************************************
      * hasp_write
      *   write to Sentinel key memory
      ********************************************************
    }

    writeln(#13#10'incrementing every byte in memory buffer');
    for i := 0 to fsize do
      inc(membuffer[i]);

    write(#13#10'writing ', fsize:4, ' bytes to memory     : ');
    status := hasp_write(handle, HASP_FILEID_RW, 0, fsize, membuffer);

    case status of

      HASP_STATUS_OK:
        writeln('OK');

      HASP_INV_HND:
        writeln('handle not active');

      HASP_INV_FILEID:
        writeln('invalid file id');

      HASP_MEM_RANGE:
        writeln('beyond memory range of attached Sentinel key');

      HASP_CONTAINER_NOT_FOUND:
        writeln('beyond memory range of attached Sentinel key');

    else
      writeln('write memory failed');

    end;
    if status <> 0 then
      Halt(1);

    {
      *************************************************
      * hasp_read
      *   read from Sentinel key memory
      *************************************************
    }

    write(#13#10'reading ', fsize:4, ' bytes from memory   : ');
    status := hasp_read(handle, HASP_FILEID_RW, 0, fsize, membuffer);

    case status of

      HASP_STATUS_OK:
        begin
          writeln('OK');
          dump(membuffer, fsize, '    ');
        end;

      HASP_INV_HND:
        writeln('handle not active');

      HASP_INV_FILEID:
        writeln('invalid file id');

      HASP_MEM_RANGE:
        writeln('beyond memory range of attached Sentinel key');

      HASP_CONTAINER_NOT_FOUND:
        writeln('key/license container not available');

    else
      writeln('read memory failed');

    end;
    if status <> 0 then
      Halt(1);
  end;

  writeln;
  writeln('Encrypting a data buffer:');
  encrypt_decrypt(handle);

  {
    *****************************************************
    * hasp_login
    *
    *   establishes a context for Sentinel services.
    *   A new handle is used to log into another feature.
    *****************************************************
  }

  write(#13#10#13#10'login to feature ', CUSTOM_FEATURE:4, '            : ');
  status := hasp_login(CUSTOM_FEATURE, PAnsiChar(vendor_code), handle2);

  case status of

    HASP_STATUS_OK:
      writeln('OK');

    HASP_FEATURE_NOT_FOUND:
      writeln('no DemoMA Sentinel key found with feature ', CUSTOM_FEATURE);

    HASP_CONTAINER_NOT_FOUND:
      writeln('key not available');

    HASP_OLD_DRIVER:
      writeln('outdated driver version installed');

    HASP_NO_DRIVER:
      writeln('Sentinel driver not installed');

    HASP_INV_VCODE:
      writeln('invalid vendor code');

  else
    writeln('failed with status ', status);

  end;
  if status <> 0 then
    Halt(1);

  writeln;
  writeln('encrypt/decrypt again to see different encryption for different features:');
  encrypt_decrypt(handle2);

  {
    *************************************************************
    * hasp_logout
    *   closes established session and releases allocated memory.
    *************************************************************
  }

  write(#13#10'logout from feature ', CUSTOM_FEATURE:4, '         : ');
  status := hasp_logout(handle2);

  case status of
    HASP_STATUS_OK:
      writeln('OK');

    HASP_INV_HND:
      writeln('failed: handle not active');

  else
    writeln('failed with status ', status);

  end;
  if status <> 0 then
    Halt(1);

  {
    *************************************************
    * hasp_get_rtc
    *   read current time from Sentinel Time key.
    *************************************************
  }

  write(#13#10'reading current time and date    : ');
  status := hasp_get_rtc(handle, time);

  case status of

    HASP_STATUS_OK:
      begin
        status := hasp_hasptime_to_datetime(time, day, month, year, hour,
          minute, second);

        case status of
          HASP_STATUS_OK:
            begin
              writeln('time: ', hour:2, ':', minute:2, ':', second:2, ' H/M/S');
              writeln('date: ':41, day:2, '/', month:2, '/', year:4, ' D/M/Y');
            end;

          HASP_INV_TIME:
            writeln('time value outside supported range');

        else
          writeln('time conversion failed');

        end;
        if status <> 0 then
          Halt(1);

      end;

    HASP_INV_HND:
      writeln('handle not active'#13#10);

    HASP_NO_TIME:
      writeln('no Sentinel Time Key connected'#13#10);

    HASP_CONTAINER_NOT_FOUND:
      writeln('key/license container not available');

  else
    writeln('could not read time from Sentinel key, status ', status);

  end;
  if status <> 0 then
    Halt(1);

  {
    *************************************************************
    * hasp_logout
    *   closes established session and releases allocated memory.
    *************************************************************
  }

  write(#13#10'logout from default feature      : ');
  status := hasp_logout(handle);

  case status of

    HASP_STATUS_OK:
      writeln('OK');

    HASP_INV_HND:
      writeln('failed: handle not active');

  else
    writeln('failed');

  end;
  if status <> 0 then
    Halt(1);

  {
    **************************************************
    * hasp_login_scope
    *   establishes a context for Sentinel services.
    *   Allows specification of serveral restrictions.
    **************************************************
  }

  writeln('restricting the license to "local" :');
  scope := '<haspscope>'#13#10 +
    ' <license_manager hostname="localhost"/>'#13#10 + 
    '</haspscope>'#13#10;
  writeln(scope);

  write('hasp_login_scope                 : ');
  status := hasp_login_scope(HASP_DEFAULT_FID, PAnsiChar(scope),
    PAnsiChar(vendor_code), handle);

  case status of

    HASP_STATUS_OK:
      writeln('OK');

    HASP_FEATURE_NOT_FOUND:
      writeln('login to default feature failed');

    HASP_CONTAINER_NOT_FOUND:
      writeln('login to default feature failed');

    HASP_OLD_DRIVER:
      writeln('outdated driver version installed');

    HASP_NO_DRIVER:
      writeln('Sentinel driver not installed');

    HASP_INV_VCODE:
      writeln('invalid vendor code');

    HASP_INV_SCOPE:
      writeln('invalid XML scope');

  else
    writeln('login to default feature failed with status ', status);

  end;
  if status <> 0 then
    Halt(1);

  write('getting information about connected keys and usage : ');

  scope := '<haspscope />'#13#10;
  view := '<haspformat root="my_custom_scope">'#13#10 + 
    '  <hasp>'#13#10 + 
    '    <attribute name="id" />'#13#10 + 
    '    <attribute name="type" />'#13#10 + 
    '    <feature>'#13#10 + 
    '      <attribute name="id" />'#13#10 +
    '      <element name="concurrency" />'#13#10 +
    '      <element name="license" />'#13#10 + 
    '      <session>'#13#10 +
    '        <element name="username" />'#13#10 +
    '        <element name="hostname" />'#13#10 +
    '        <element name="ip" />'#13#10 +
    '        <element name="apiversion" />'#13#10 + 
    '      </session>'#13#10 +
    '    </feature>'#13#10 + 
    '  </hasp>'#13#10 + 
    '</haspformat>'#13#10;

  status := hasp_get_info(PAnsiChar(scope), PAnsiChar(view),
    PAnsiChar(vendor_code), info);

  case status of

    HASP_STATUS_OK:
      begin
        writeln('OK');
        writeln(info);
        {
          *****************************************************************
          * hasp_free
          *   frees memory allocated by hasp_get_info, hasp_get_sessioninfo
          *   or hasp_update (if an acknowledge was requested)
          *****************************************************************
        }
        hasp_free(info);
      end;

    HASP_INV_FORMAT:
      writeln('invalid XML info format');

    HASP_INV_SCOPE:
      writeln('invalid XML scope');

  else
    writeln('hasp_get_info failed with status ', status);

  end;
  if status <> 0 then
    Halt(1);

  {
    ************************************************************
    * hasp_logout
    *   closes established session and releases allocated memory
    ************************************************************
  }

  write(#13#10'logout                           : ');
  status := hasp_logout(handle);

  case status of

    HASP_STATUS_OK:
      writeln('OK');

    HASP_INV_HND:
      writeln('failed: handle not active');

  else
    writeln('failed');

  end;
  if status <> 0 then
    Halt(1);

  writeln(#13#10'sample finished successfully!');
  ExitCode := 0;

end.
