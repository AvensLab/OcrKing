unit MainForm;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, IdHTTP, StdCtrls, IdMultipartFormData, XMLDoc, XMLIntf,
  IdTCPConnection, IdTCPClient, IdBaseComponent, IdComponent, IdIOHandler,
  IdIOHandlerSocket, IdSSLOpenSSL;

const
  API_KEY='550f10baf8e9788e7frzMsMdDAm2xGvYJSYU8kVWF/Pac93CHyd1HrAR/aH7XzVcUCgfg2nkPX+eJFcw';//��Ҫ������һ��apikey
  API_HOST='http://lab.ocrking.com/ok.html';//�ύ�жϵ���ҳ
  CONST_VALID_IMAGE='https://www.bestpay.com.cn/api/captcha/getCode?1408294248050'; //��ȡ��֤���ҳ��

type
  //ͼƬ����
  TOcrService=(osPassages,osPDF,osPhoneNumber,osPrice,osNumber,osCaptcha,osBarcode,osPDFImage);
  //����
  TOcrLanguage=(olEng,olSim,olTra,olJpn,olKor);
  //�ַ���
  TOcrCharset=(ocEnglish,ocNumber,ocLowEnglish,ocUpEnglish,ocNumLowEng,ocNumUpEng
    ,ocAllEng,ocNumAllEng,ocEngChar,ocEmailWeb,ocShopPrice,ocPhoneNumber,ocFomula);
  TfrmMain = class(TForm)
    btn1: TButton;
    OD: TOpenDialog;
    cbbService: TComboBox;
    cbbLanguage: TComboBox;
    cbbCharset: TComboBox;
    mm: TMemo;
    btn2: TButton;
    idslhndlrsckt1: TIdSSLIOHandlerSocket;
    FHttp: TIdHTTP;
    procedure FormCreate(Sender: TObject);
    procedure btn1Click(Sender: TObject);
    procedure btn2Click(Sender: TObject);
  private
    { Private declarations }
  public
    FAppPath:String;
  end;
  function OcrLanguage2Str(const AOcrCharset:TOcrLanguage):string;//����ö��ת�ַ���
  function OcrService2Str(const AService:TOcrService):string;//��������ö��ת�ַ���
  function UrlEncode(const ASrc: string): string;//Url����
var
  frmMain: TfrmMain;

implementation

{$R *.dfm}

function UrlEncode(const ASrc: string): string;
const
  UnsafeChars = '*#%<>+ []';  //do not localize
var
  i: Integer;
begin
  Result := '';    //Do not Localize
  for i := 1 to Length(ASrc) do begin
    if (AnsiPos(ASrc[i], UnsafeChars) > 0) or (ASrc[i]< #32) or (ASrc[i] > #127) then begin
      Result := Result + '%' + IntToHex(Ord(ASrc[i]), 2);  //do not localize
    end else begin
      Result := Result + ASrc[i];
    end;
  end;
end;

function OcrLanguage2Str(const AOcrCharset:TOcrLanguage):string;
begin
  case AOcrCharset of
    olEng:Result:='eng';
    olSim:Result:='sim';
    olTra:Result:='tra';
    olJpn:Result:='jpn';
    olKor:Result:='kor';
  end;
end;

function OcrService2Str(const AService:TOcrService):string;
begin
  case AService of
    osPassages:Result:='OcrKingForPassages';
    osPDF:Result:='OcrKingForPDF';
    osPhoneNumber:Result:='OcrKingForPhoneNumber';
    osPrice:Result:='OcrKingForPrice';
    osNumber:Result:='OcrKingForNumber';
    osCaptcha:Result:='OcrKingForCaptcha';
    osBarcode:Result:='BarcodeDecode';
    osPDFImage:Result:='PDFToImage';
  end;
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  FAppPath:=ExtractFilePath(Application.ExeName);
  if FAppPath[Length(FAppPath)]<>'\' then FAppPath:=FAppPath+'\';
end;

procedure TfrmMain.btn1Click(Sender: TObject);
var
  ms:TIdMultiPartFormDataStream;
  sAll,sUrl,sService,sLanguage,sCharset,sApiKey,sType,sResponse:String;
  xmlDocument:IXmlDocument;
  xmlNode:IXMLNode;
begin
  if OD.Execute then
    begin
      FHttp.Disconnect;
      mm.Clear;
      ms:=TIdMultiPartFormDataStream.Create;     
      try
        sUrl:='?url=';//�����ֱ����֤����ͼƬ������д��֤�����ɵ�ַ
        sService:='&service='+OcrService2Str(TOcrService(cbbService.ItemIndex));//��Ҫʶ�������
        sLanguage:='&language='+OcrLanguage2Str(TOcrLanguage(cbbLanguage.ItemIndex));//����
        sCharset:='&charset='+IntToStr(cbbCharset.ItemIndex);//�ַ���
        sApiKey:='&apiKey='+API_KEY;//�������apikey
        sType:='&type='+CONST_VALID_IMAGE;//������֤��ԭʼ������ַ
        sAll:=API_HOST+sUrl+sService+sLanguage+sCharset+sApiKey+sType;
        ms.AddFile('ocrfile',OD.FileName,'');//�ϴ����ļ������Ʊ���Ϊocrfile
        //������ύ�Ĳ��������⣬���Գ����ֶ����������ֵ
        //FHttp.Request.ContentType:='multipart/form-data';
        //FHttp.Request.Host:='lab.ocrking.com';
        //FHttp.Request.Accept:='*/*';
        //FHttp.Request.ContentLength:=ms.Size;
        //FHttp.Request.Connection:='Keep-Alive';
        sResponse:=UTF8ToAnsi(FHttp.Post(sAll,ms));
        mm.Lines.Append(sResponse);
        xmlDocument:=LoadXMLData(sResponse);
        xmlNode:=xmlDocument.ChildNodes.FindNode('Results').ChildNodes.FindNode('ResultList').ChildNodes.FindNode('Item');
        mm.Lines.Add('״̬:'+xmlNode.ChildNodes.FindNode('Status').NodeValue);
        mm.Lines.Add('���:'+xmlNode.ChildNodes.FindNode('Result').NodeValue);
      finally
        ms.Free;
      end;
    end;
end;



procedure TfrmMain.btn2Click(Sender: TObject);
var
  ms:TMemoryStream;
begin
  ms:=TMemoryStream.Create;
  try
    FHttp.Disconnect;       
    FHttp.Get(CONST_VALID_IMAGE,ms);
    ms.SaveToFile(FAppPath+'yzm.png');
  finally
    ms.Free;
  end;
end;

end.