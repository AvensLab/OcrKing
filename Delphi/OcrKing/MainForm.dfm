object frmMain: TfrmMain
  Left = 907
  Top = 279
  Width = 298
  Height = 422
  Caption = 'Delphi for OrcKing'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object btn1: TButton
    Left = 160
    Top = 16
    Width = 90
    Height = 25
    Caption = #25552#20132#39564#35777#30721
    TabOrder = 0
    OnClick = btn1Click
  end
  object cbbService: TComboBox
    Left = 16
    Top = 48
    Width = 100
    Height = 21
    Style = csDropDownList
    ItemHeight = 13
    ItemIndex = 5
    TabOrder = 1
    Text = #39564#35777#30721#31867
    Items.Strings = (
      #38271#31687#20869#23481
      'PDF'#35782#21035
      #25163#26426#30005#35805
      #21830#22478#20215#26684
      #32431#25968#23383#31867
      #39564#35777#30721#31867
      #26465#24418#20108#32500#30721
      'PDF'#36716#22270#29255)
  end
  object cbbLanguage: TComboBox
    Left = 152
    Top = 48
    Width = 100
    Height = 21
    Style = csDropDownList
    ItemHeight = 13
    ItemIndex = 0
    TabOrder = 2
    Text = #33521#35821
    Items.Strings = (
      #33521#35821
      #31616#20307
      #32321#20307
      #26085#35821
      #38889#35821)
  end
  object cbbCharset: TComboBox
    Left = 16
    Top = 80
    Width = 145
    Height = 21
    Style = csDropDownList
    ItemHeight = 13
    ItemIndex = 7
    TabOrder = 3
    Text = #25968#23383#22823#20889#23567#20889
    Items.Strings = (
      #25152#26377#33521#25991#23383#31526
      #25152#26377#32431#25968#23383
      #23567#20889#33521#25991#23383#27597
      #22823#20889#33521#25991#23383#27597
      #25968#23383#23567#20889#23383#27597
      #25968#23383#22823#20889#23383#27597
      #22823#20889#23567#20889#23383#27597
      #25968#23383#22823#20889#23567#20889
      #24120#29992#33521#25991#23383#31526
      #32593#22336#21644#37038#20214#31867
      '$'#65509#21830#22478#20215#26684
      #25163#26426#30005#35805#21495#31867
      #25968#23398#20844#24335#35745#31639)
  end
  object mm: TMemo
    Left = 16
    Top = 112
    Width = 249
    Height = 257
    ScrollBars = ssVertical
    TabOrder = 4
  end
  object btn2: TButton
    Left = 23
    Top = 16
    Width = 90
    Height = 25
    Caption = #33719#21462#39564#35777#30721
    TabOrder = 5
    OnClick = btn2Click
  end
  object OD: TOpenDialog
    Left = 296
    Top = 16
  end
  object idslhndlrsckt1: TIdSSLIOHandlerSocket
    SSLOptions.Method = sslvSSLv3
    SSLOptions.Mode = sslmUnassigned
    SSLOptions.VerifyMode = []
    SSLOptions.VerifyDepth = 0
    Left = 160
    Top = 80
  end
  object FHttp: TIdHTTP
    IOHandler = idslhndlrsckt1
    MaxLineAction = maException
    ReadTimeout = 0
    AllowCookies = True
    ProxyParams.BasicAuthentication = False
    ProxyParams.ProxyPort = 0
    Request.ContentLength = -1
    Request.ContentRangeEnd = 0
    Request.ContentRangeStart = 0
    Request.ContentType = 'text/html'
    Request.Accept = 'text/html, */*'
    Request.BasicAuthentication = False
    Request.UserAgent = 'Mozilla/3.0 (compatible; Indy Library)'
    HTTPOptions = [hoForceEncodeParams]
    Left = 200
    Top = 80
  end
end
