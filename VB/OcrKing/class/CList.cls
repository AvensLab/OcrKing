VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CList"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'//////////////////////////////////////////////////////////////////////////////
'@@summary 支持简单的列表，支持特定类型
'@@require
'@@reference
'@@license
'@@author
'@@create
'@@modify
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      公有声明
'//
'//////////////////////////////////////////////////////////////////////////////


'------------------------------------------------------------------------------
'       接口继承
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有常量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有数据类型
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有变量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有API
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       事件声明
'------------------------------------------------------------------------------


'//////////////////////////////////////////////////////////////////////////////
'//
'//      私有声明
'//
'//////////////////////////////////////////////////////////////////////////////


'------------------------------------------------------------------------------
'       私有常量
'------------------------------------------------------------------------------
Private Const INIT_SIZE = 1024
Private Const INIT_GAP_SIZE = 128

'------------------------------------------------------------------------------
'       私有数据类型
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       私有变量
'------------------------------------------------------------------------------
Private mArr() As Variant
Private mCount As Long
Private mSize As Long

'------------------------------------------------------------------------------
'       属性变量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       私有API
'------------------------------------------------------------------------------


'//////////////////////////////////////////////////////////////////////////////
'//
'//      类
'//
'//////////////////////////////////////////////////////////////////////////////


'------------------------------------------------------------------------------
'       初始化
'------------------------------------------------------------------------------
Private Sub Class_Initialize()
  mCount = 0
  mSize = INIT_SIZE
  ReDim mArr(1 To mSize)
End Sub


'------------------------------------------------------------------------------
'       销毁
'------------------------------------------------------------------------------
Private Sub Class_Terminate()

End Sub


'//////////////////////////////////////////////////////////////////////////////
'//
'//      事件处理
'//
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      私有属性
'//
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      私有方法
'//
'//////////////////////////////////////////////////////////////////////////////
Private Sub resize()
  mSize = mSize + INIT_GAP_SIZE
  ReDim Preserve mArr(1 To mSize)
End Sub

'//////////////////////////////////////////////////////////////////////////////
'//
'//      继承实现
'//
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      公有属性
'//
'//////////////////////////////////////////////////////////////////////////////
Public Property Get Item(ByVal index As Long) As Variant
  If index > mCount Or index < 1 Then
    Err.Raise 10032301, , "[SunSoft]索引超出范围"
  End If
  Item = mArr(index)
End Property

Public Property Let Item(ByVal index As Long, value As Variant)
  If index > mCount Or index < 1 Then
    Err.Raise 10032301, , "[SunSoft]索引超出范围"
  End If
  mArr(index) = value
End Property

Public Property Get Count() As Long
  Count = mCount
End Property
'//////////////////////////////////////////////////////////////////////////////
'//
'//      公有方法
'//
'//////////////////////////////////////////////////////////////////////////////
Public Function Add(ByVal value As Variant) As CList
  If mCount >= mSize Then
    Call resize
  End If
  mCount = mCount + 1
  mArr(mCount) = value
  Set Add = Me
End Function

Public Function ToArray() As String()
  Dim arr() As String, i As Long
  
  If mCount = 0 Then
    ToArray = arr
    Exit Function
  End If
  
  ReDim arr(mCount - 1)
  For i = 0 To mCount - 1
    arr(i) = CStr(mArr(i + 1))
  Next i
  ToArray = arr
End Function

Public Function Join(Optional ByVal Separator As String = "", Optional ByVal Start As Long = 1) As String
  Dim joins As String, i As Long
  
  If mCount = 0 Then
    Join = ""
    Exit Function
  End If

  For i = Start To mCount
    joins = joins & Separator & CStr(mArr(i))
  Next i
  
  joins = Mid(joins, Len(Separator) + 1)
  
  Join = joins
End Function

Public Function WrapJoin(Optional ByVal Separator As String = "", Optional ByVal Wraper As String = "", Optional ByVal Start As Long = 1) As String
  Dim joins As String, i As Long
  
  If mCount = 0 Then
    WrapJoin = ""
    Exit Function
  End If

  For i = Start To mCount
    joins = joins & Separator & Wraper & CStr(mArr(i)) & Wraper
  Next i
  
  joins = Mid(joins, Len(Separator) + 1)
  
  WrapJoin = joins
End Function

Public Sub Clear()
  Call Class_Initialize
End Sub

Public Function LoadFromString(ByVal DataString As String, ByVal Delimiter As String) As CList
  Dim mArr() As String, i As Long
  Call Clear
  If DataString = "" Then
    Set LoadFromString = Me
    Exit Function
  End If
  mArr = Split(DataString, Delimiter)
  For i = 0 To UBound(mArr)
    Add mArr(i)
  Next i
  Set LoadFromString = Me
End Function
