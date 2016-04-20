VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "识别"
   ClientHeight    =   4740
   ClientLeft      =   60
   ClientTop       =   405
   ClientWidth     =   7815
   LinkTopic       =   "Form1"
   ScaleHeight     =   4740
   ScaleWidth      =   7815
   StartUpPosition =   3  '窗口缺省
   Begin VB.CommandButton btnGetCode 
      Caption         =   "识别验证码temp.jpg"
      Height          =   495
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   1935
   End
   Begin VB.TextBox Text1 
      Height          =   3975
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   0
      Top             =   720
      Width           =   7575
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub btnGetCode_Click()
  Dim ocr As New OcrKing
  ocr.Key = "这里写你的key"
  Text1.Text = ocr.GetCodeFromFile(App.Path & "\temp.jpg")
End Sub
