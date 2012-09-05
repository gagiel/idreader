
#define TERMB_API __declspec(dllexport)

extern TERMB_API int PASCAL InitComm(int port);
extern TERMB_API int PASCAL InitCommExt();
extern TERMB_API int PASCAL CloseComm();
extern TERMB_API int PASCAL Authenticate();
extern TERMB_API int PASCAL Read_Content( int active);
extern TERMB_API int PASCAL Read_Content_Path(char* cPath, int active);
extern TERMB_API int PASCAL GetDeviceID( char * pMsg );
extern TERMB_API BSTR PASCAL GetSAMID();
extern TERMB_API int PASCAL GetPhoto(char * Wlt_File);
extern TERMB_API void PASCAL MfrInfo(char * cDeviceType,char * cDeviceCategory,char * cDeviceName,char * cMfr);