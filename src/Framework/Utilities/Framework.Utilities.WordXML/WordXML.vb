Imports System.IO.FileStream
Imports System.IO
Imports System.Xml
Imports DreamCube.Foundation.Basic.Utility
'Imports System.Web.HttpContext
'Imports System.Web


Public Class WordXML
    Public xXmlDoc As XmlDocument
    Public sFileToStr, sFileSavePath As String
    Public xRootNode As XmlElement
    Public xMainDocEle As XmlElement
    Public xBodyEle As XmlElement

    Sub New()

    End Sub

    ''' <summary>
    ''' 替换修改带格式的值
    ''' </summary>
    ''' <param name="sBookMarkName"></param>
    ''' <param name="sHtml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function WriteHtmlValueEx(ByVal sBookMarkName As String, ByVal sHtml As String) As Boolean
        sFileToStr = xXmlDoc.InnerXml
        If String.IsNullOrEmpty(sBookMarkName) Or sFileToStr.IndexOf(sBookMarkName) <= 0 Then
            Return False
        End If
        Dim xWordPartToIn As XmlElement = GetWPByBookMarkName(sBookMarkName)
        LoadHtmlTOWP(sHtml, xWordPartToIn)
        Return True
    End Function


    ''' <summary>
    ''' 替换修改带格式的值
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <param name="sHtml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function WriteHtmlValue(ByVal sName As String, ByVal sHtml As String) As Boolean
        sFileToStr = xXmlDoc.InnerXml
        If String.IsNullOrEmpty(sName) Or sFileToStr.IndexOf(sName) <= 0 Then
            Return False
        End If
        Dim xWordPartToIn As XmlElement = GetWPByText(sName)
        LoadHtmlTOWP(sHtml, xWordPartToIn)
        Return True
    End Function


    ''' <summary>
    ''' 替换修改无格式文本修改值
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function WriteValue(ByVal sName As String, ByVal sValue As String) As Boolean
        sFileTostr = xXmlDoc.InnerXml
        If String.IsNullOrEmpty(sName) Or sFileTostr.IndexOf(sName) <= 0 Then
            Return False
        End If
        sFileToStr = sFileToStr.Replace(sName, sValue)
        IniteXmlByStr(sFileToStr)
        Return True
    End Function

    Public Function WriteValueAtBookMark(ByVal sBookMarkName As String, ByVal sValue As String) As Boolean
        Dim xBookMarkNode As XmlElement = GetBookMarkNodeByName(sBookMarkName)
        If xBookMarkNode Is Nothing Then
            Return False
        End If
        Dim xXmlCurWR As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        If Not (xBookMarkNode.PreviousSibling() Is Nothing) Then
            If xBookMarkNode.PreviousSibling().Name = "w:r" And CType(xBookMarkNode.PreviousSibling(), XmlElement).GetElementsByTagName("w:rPr").Count() > 0 Then
                xXmlCurWR.AppendChild(CType(xBookMarkNode.PreviousSibling(), XmlElement).GetElementsByTagName("w:rPr")(0).Clone())
            Else
                Dim xCurParentNode As XmlElement = xBookMarkNode.ParentNode
                If xCurParentNode.GetElementsByTagName("w:pPr").Count() > 0 Then
                    If CType(xCurParentNode.GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr").Count() > 0 Then
                        xXmlCurWR.AppendChild(CType(xCurParentNode.GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr")(0).Clone())
                    End If
                End If
            End If
        Else
            Dim xCurParentNode As XmlElement = xBookMarkNode.ParentNode
            If xCurParentNode.GetElementsByTagName("w:pPr").Count() > 0 Then
                If CType(xCurParentNode.GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr").Count() > 0 Then
                    xXmlCurWR.AppendChild(CType(xCurParentNode.GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr")(0).Clone())
                End If
            End If
        End If
        xBookMarkNode.ParentNode().InsertAfter(xXmlCurWR, xBookMarkNode)
        AppendTextToWr(xXmlCurWR, sValue)
        Return True
    End Function

    Public Function GetBookMarkNodeByName(ByVal sBookMarkName As String) As XmlElement
        Dim WBookMarkList As XmlNodeList = xBodyEle.GetElementsByTagName("w:bookmarkStart")
        If WBookMarkList.Count() <= 0 Then
            Return Nothing
        End If
        For i As Integer = 0 To WBookMarkList.Count() - 1
            If CType(WBookMarkList(i), XmlElement).GetAttribute("w:name") = sBookMarkName Then
                Return WBookMarkList(i)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' 获取所有书签
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllBookMarks() As List(Of String)
        Try
            Dim WBookMarkList As XmlNodeList = xBodyEle.GetElementsByTagName("w:bookmarkStart")
            If WBookMarkList.Count() <= 0 Then
                Return Nothing
            End If
            Dim aBookMarkList As List(Of String) = New List(Of String)
            For i As Integer = 0 To WBookMarkList.Count() - 1
                aBookMarkList.Add(CType(WBookMarkList(i), XmlElement).GetAttribute("w:name"))
            Next
            Return aBookMarkList
        Catch ex As Exception
            MyLog.MakeLog(ex)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' 根据书签定位到行
    ''' </summary>
    ''' <param name="sBookMarkName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWPByBookMarkName(ByVal sBookMarkName As String) As XmlElement
        Dim WBookMarkList As XmlNodeList = xBodyEle.GetElementsByTagName("w:bookmarkStart")
        If WBookMarkList.Count() <= 0 Then
            Return Nothing
        End If
        For i As Integer = 0 To WBookMarkList.Count() - 1
            If CType(WBookMarkList(i), XmlElement).GetAttribute("w:name") = sBookMarkName Then
                Return WBookMarkList(i).ParentNode()
            End If
        Next
        Return Nothing
    End Function


    ''' <summary>
    ''' 通过文本内容定位到文本所在行
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWPByText(ByVal sName) As XmlElement
        Dim WPList As XmlNodeList = xBodyEle.GetElementsByTagName("w:p")
        If WPList Is Nothing Or WPList.Count() <= 0 Then
            Return Nothing
        End If
        Dim i As Integer = 0
        While i < WPList.Count()
            If WPList(i).InnerText.IndexOf(sName) >= 0 Then
                Return WPList(i)
            End If
            i += 1
        End While
        Return Nothing
    End Function
    ''' <summary>
    ''' 加载转换html后的内容到文档特定段落“p”位置替换
    ''' </summary>
    ''' <param name="sHtml"></param>
    ''' <param name="WPNode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadHtmlTOWP(ByVal sHtml As String, ByRef WPNode As XmlNode) As Boolean
        If WPNode Is Nothing Or String.IsNullOrEmpty(sHtml.Trim()) Then
            Return False
        End If
        Dim sXML_Html As String = "<htmlroot>" & sHtml & "</htmlroot>"
        Dim html_doc As XmlDocument = New XmlDocument()
        html_doc.LoadXml(sXML_Html)
        Dim html_root As XmlElement = html_doc.ChildNodes()(0)
        If html_root.ChildNodes().Count() <= 0 Then
            Return False
        End If
        Dim xResult As XmlNode = ConvertHtmlToWordXml(html_root)
        Dim xCurNode As XmlNode = WPNode
        For i As Integer = 0 To xResult.ChildNodes().Count() - 1
            WPNode.ParentNode().InsertBefore(xResult.ChildNodes()(i).Clone(), WPNode)
        Next
        WPNode.ParentNode.RemoveChild(WPNode)
        Return True
    End Function

    ''' <summary>
    ''' 把转换后HTML内容插入特定位置的前面
    ''' </summary>
    ''' <param name="sHtml"></param>
    ''' <param name="WPNode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function LoadHtmlBeforeWP(ByVal sHtml As String, ByRef WPNode As XmlNode) As Boolean
        If WPNode Is Nothing Or String.IsNullOrEmpty(sHtml.Trim()) Then
            Return False
        End If
        Dim sXML_Html As String = "<htmlroot>" & sHtml & "</htmlroot>"
        Dim html_doc As XmlDocument = New XmlDocument()
        html_doc.LoadXml(sXML_Html)
        Dim html_root As XmlElement = html_doc.ChildNodes()(0)
        If html_root.ChildNodes().Count() <= 0 Then
            Return False
        End If
        Dim xResult As XmlNode = ConvertHtmlToWordXml(html_root)
        Dim xCurNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        WPNode.ParentNode.InsertBefore(xCurNode, WPNode)
        For i As Integer = 0 To xResult.ChildNodes().Count() - 1
            WPNode.ParentNode().InsertBefore(xResult.ChildNodes()(i).Clone(), xCurNode)
        Next
        WPNode.ParentNode.RemoveChild(xCurNode)
        Return True
    End Function

    ''' <summary>
    ''' 把转换后HTML内容插入特定位置的后面
    ''' </summary>
    ''' <param name="sHtml"></param>
    ''' <param name="WPNode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function LoadHtmlAfterWP(ByVal sHtml As String, ByRef WPNode As XmlNode) As Boolean
        If WPNode Is Nothing Or String.IsNullOrEmpty(sHtml.Trim()) Then
            Return False
        End If
        Dim sXML_Html As String = "<htmlroot>" & sHtml & "</htmlroot>"
        Dim html_doc As XmlDocument = New XmlDocument()
        html_doc.LoadXml(sXML_Html)
        Dim html_root As XmlElement = html_doc.ChildNodes()(0)
        If html_root.ChildNodes().Count() <= 0 Then
            Return False
        End If
        Dim xResult As XmlNode = ConvertHtmlToWordXml(html_root)
        Dim xCurNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        WPNode.ParentNode.InsertAfter(xCurNode, WPNode)
        For i As Integer = 0 To xResult.ChildNodes().Count() - 1
            WPNode.ParentNode().InsertBefore(xResult.ChildNodes()(i).Clone(), xCurNode)
        Next
        WPNode.ParentNode.RemoveChild(xCurNode)
        Return True
    End Function

    ''' <summary>
    ''' 查找Root下名称为PkgName的元素节点
    ''' </summary>
    ''' <param name="xParentEle"></param>
    ''' <param name="PkgName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEleByPkgName(ByRef xParentEle As XmlElement, ByRef PkgName As String) As XmlElement
        If xParentEle Is Nothing Then
            Return Nothing
        End If
        Dim xChildList As XmlNodeList = xParentEle.ChildNodes()
        Dim n As Integer = xChildList.Count()
        If n = 0 Then
            Return Nothing
        End If
        Dim i As Integer = 0
        While i < n
            If xChildList(i).Attributes()("pkg:name").Value = PkgName Then
                Return xChildList(i)
            End If
            i += 1
        End While
    End Function

    ''' <summary>
    ''' base64编码
    ''' </summary>
    ''' <param name="bt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToBase64(ByRef bt() As Byte) As String
        'Dim aBase64Char1(Math.Ceiling(bt.Length / 3D) * 4) As Char
        Dim iCh1Len As Integer = (Fix(bt.Length / 3D)) * 4
        iCh1Len += IIf((bt.Length Mod 3) = 0, 0, 4)
        Dim aBase64Char1(iCh1Len) As Char
        Dim aBase64Char2(iCh1Len + (Fix(iCh1Len / 76) * 2)) As Char
        Convert.ToBase64CharArray(bt, 0, bt.Length, aBase64Char1, 0)
        Dim i As Integer = 0
        Dim j As Integer = 0
        While i < aBase64Char1.Length
            aBase64Char2(j) = aBase64Char1(i)
            If ((i + 1) Mod 76) = 0 Then
                j += 1
                aBase64Char2(j) = Chr(10)
                j += 1
                aBase64Char2(j) = Chr(13)
            End If
            j += 1
            i += 1
        End While
        Dim sResult As String = New String(aBase64Char2, 0, aBase64Char2.Length() - 1)
        Return sResult
    End Function
    Public Function ToBase64(ByRef sFileFullPath As String) As String

        Dim bt() As Byte = File.ReadAllBytes(sFileFullPath)
        If (bt.Length = 0) Then
            Return ""
        End If
        Return ToBase64(bt)

    End Function

    ''' <summary>
    ''' 移除所有空白页
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveAllEmptyPage() As Boolean
        Dim WPList As XmlNodeList = xBodyEle.GetElementsByTagName("w:p")
        If WPList.Count() <= 0 Then
            Return True
        End If
        For i As Integer = 0 To WPList.Count() - 1
            If Not WPList(i) Is Nothing Then
                Dim xChildNode As XmlNodeList = CType(WPList(i), XmlElement).GetElementsByTagName("w:br")
                If xChildNode.Count() > 0 Then
                    For j As Integer = 0 To xChildNode.Count() - 1
                        Dim sType As String = CType(xChildNode(j), XmlElement).GetAttribute("w:type")
                        If sType.Equals("page") Then
                            WPList(i).ParentNode().RemoveChild(WPList(i))
                        End If
                    Next
                End If
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' 移除空白页
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveAllEmptyPageEx2() As Boolean
        Dim WPList As XmlNodeList = xBodyEle.GetElementsByTagName("w:p")
        If WPList.Count() <= 0 Then
            Return True
        End If
        For i As Integer = 0 To WPList.Count() - 1
            If Not WPList(i) Is Nothing Then
                Dim xChildNode As XmlNodeList = CType(WPList(i), XmlElement).GetElementsByTagName("w:br")
                If xChildNode.Count() > 0 Then
                    For j As Integer = 0 To xChildNode.Count() - 1
                        Dim sType As String = CType(xChildNode(j), XmlElement).GetAttribute("w:type")
                        If sType.Equals("page") Then
                            Dim xPreSibNode As XmlElement = xChildNode(j).PreviousSibling()
                            If Not xPreSibNode Is Nothing Then
                                If xPreSibNode.Name = "w:lastRenderedPageBreak" Then
                                    WPList(i).ParentNode().RemoveChild(WPList(i))
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' 移除空白页
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveAllEmptyPage3() As Boolean
        Dim xAllBodyNode As XmlNodeList = xBodyEle.ChildNodes()
        Dim BeginIndex As Integer = 0
        Dim EndIndex As Integer = -1
        Dim sInNerText As String = ""
        For i As Integer = 0 To xAllBodyNode.Count() - 1
            If Not xAllBodyNode(i) Is Nothing Then
                Dim xChildNode As XmlNodeList = CType(xAllBodyNode(i), XmlElement).GetElementsByTagName("w:lastRenderedPageBreak")
                If xChildNode.Count() > 0 Then
                    If String.IsNullOrEmpty(sInNerText) Then
                        For j As Integer = BeginIndex To EndIndex
                            xAllBodyNode(j).ParentNode().RemoveChild(xAllBodyNode(j))
                        Next
                        If EndIndex < BeginIndex Then
                            xAllBodyNode(BeginIndex).ParentNode().RemoveChild(xAllBodyNode(BeginIndex))
                        End If
                    End If
                    Dim sTEST As String = xAllBodyNode(i).InnerText()
                    sInNerText = ""
                    sInNerText = sInNerText & xAllBodyNode(i).InnerText()
                    BeginIndex = i
                    EndIndex = i - 1
                Else
                    EndIndex = EndIndex + 1

                    sInNerText = sInNerText & xAllBodyNode(i).InnerText()
                End If
            End If
        Next
        If EndIndex = xAllBodyNode.Count() Then
            If String.IsNullOrEmpty(sInNerText) Then
                For j As Integer = BeginIndex To EndIndex
                    xAllBodyNode(j).ParentNode().RemoveChild(xAllBodyNode(j))
                Next
                If EndIndex < BeginIndex Then
                    xAllBodyNode(BeginIndex).ParentNode().RemoveChild(xAllBodyNode(BeginIndex))
                End If
            End If
        End If
        Return True
    End Function

    ''' <summary>
    ''' 移除所有空白页
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveAllEmptyPageEx() As Boolean
        Return WriteValue("<w:br w:type=""page""/>", "")
    End Function

    ''' <summary>
    ''' 初始化函数
    ''' </summary>
    ''' <param name="sFileFullPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IniteXmlByFullPath(ByVal sFileFullPath As String) As Boolean
        If xXmlDoc Is Nothing Then
            xXmlDoc = New XmlDocument()
        End If
        Try
            sFileToStr = File.ReadAllText(sFileFullPath)
            If String.IsNullOrEmpty(sFileFullPath) Then
                sFileSavePath = sFileFullPath
            End If
            If Not IniteXmlByStr(sFileToStr) Then
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function IniteXmlByStr(ByVal sXmlStr As String) As Boolean
        If xXmlDoc Is Nothing Then
            xXmlDoc = New XmlDocument()
        End If
        Try
            sFileToStr = sXmlStr
            xXmlDoc.LoadXml(sFileToStr)
            xRootNode = xXmlDoc.GetElementsByTagName("pkg:package")(0)
            xMainDocEle = GetEleByPkgName(xRootNode, "/word/document.xml")
            xBodyEle = xMainDocEle.ChildNodes()(0).ChildNodes()(0).ChildNodes()(0)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' 保存数据
    ''' </summary>
    ''' <param name="sSavePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveXml(ByVal sSavePath As String) As Boolean
        Try
            sFileSavePath = sSavePath
            xXmlDoc.Save(sFileSavePath)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''HTML转换为word的XML相关函数
    ''' <summary>
    ''' 转换html为word格式的xml，返回顶级节点为HToXResult的XML,其中Html被存放在顶级节点htmlroot中
    ''' </summary>
    ''' <param name="ParentNode">html的顶级节点htmlroot</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertHtmlToWordXml(ByVal ParentNode As XmlNode) As XmlNode
        Dim xResult As XmlNode = xXmlDoc.CreateElement("HToXResult")
        Dim ChildList As XmlNodeList = ParentNode.ChildNodes()
        Dim xCurNode As XmlNode
        If ChildList(0).Name = "table" Then
            xCurNode = xXmlDoc.CreateElement("w", "tbl", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            xResult.AppendChild(xCurNode)
            ConvertTable(xCurNode, ChildList(0))
        Else
            xCurNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") 'xXmlDoc.CreateElement("w:p")
            xResult.AppendChild(xCurNode)
            ConvertWP(xCurNode, ChildList(0))
        End If

        Dim i As Integer = 1
        Dim n As Integer = ChildList.Count()
        While i < n
            SelectWPByName(xResult, ChildList(i), xCurNode, Nothing)
            'SelectWPByName(xCurNode.ParentNode(), ChildList(i), xCurNode, Nothing)
            i += 1
        End While
        Return xResult
    End Function

    ''' <summary>
    ''' 根据html标签名称内容选择转换类型，仅适用于判断执行段落还是table转换函数
    ''' </summary>
    ''' <param name="xXmlTopNode">当前位置的上级</param>
    ''' <param name="xHtmlCode"></param>
    ''' <param name="xCurNode">word xml当前位置</param>
    ''' <remarks></remarks>
    Private Sub SelectWPByName(ByVal xXmlTopNode As XmlNode, ByVal xHtmlCode As XmlNode, ByRef xCurNode As XmlElement, ByVal xpPrele As XmlElement)
        Dim sHtmlTagName As String = xHtmlCode.Name
        If sHtmlTagName = "table" Then
            xCurNode = xXmlDoc.CreateElement("w", "tbl", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            If Not (xpPrele Is Nothing) Then
                xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tblPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                AppendTBStyle(xCurNode.GetElementsByTagName("w:tblPr")(0), xHtmlCode.ParentNode())
            End If
            xXmlTopNode.AppendChild(xCurNode)
            ConvertTable(xCurNode, xHtmlCode)
        ElseIf sHtmlTagName = "b" Or sHtmlTagName = "i" Or sHtmlTagName = "u" Or sHtmlTagName = "span" Or sHtmlTagName = "font" Or sHtmlTagName = "strong" Or sHtmlTagName = "img" Or sHtmlTagName = "a" Then
            If Not (xHtmlCode.PreviousSibling Is Nothing) Then

                If xHtmlCode.PreviousSibling.Name = "div" Or xHtmlCode.PreviousSibling.Name = "p" Or xHtmlCode.PreviousSibling.Name = "table" Then
                    xCurNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                    If Not (xpPrele Is Nothing) Then
                        xCurNode.AppendChild(xpPrele.Clone())
                    End If
                    xXmlTopNode.AppendChild(xCurNode)
                End If
            End If
            ConvertWP(xCurNode, xHtmlCode)
        ElseIf sHtmlTagName = "br" Then
            Dim xNewNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            If Not (xpPrele Is Nothing) Then
                xNewNode.AppendChild(xpPrele.Clone())
            End If
            xXmlTopNode.AppendChild(xNewNode)
            xCurNode = xNewNode
        ElseIf xHtmlCode.GetType().ToString() = "System.Xml.XmlText" Then
            If Not xHtmlCode.PreviousSibling Is Nothing Then
                If xHtmlCode.PreviousSibling.Name = "div" Or xHtmlCode.PreviousSibling.Name = "p" Or xHtmlCode.PreviousSibling.Name = "table" Then
                    xCurNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                    If Not (xpPrele Is Nothing) Then
                        xCurNode.AppendChild(xpPrele.Clone())
                    End If
                    xXmlTopNode.AppendChild(xCurNode)
                End If
            End If
            ConvertWP(xCurNode, xHtmlCode)
        Else 'sHtmlTagName = "div" Or sHtmlTagName = "p" Or sHtmlTagName = "br" Then
            xCurNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            If Not (xpPrele Is Nothing) Then
                xCurNode.AppendChild(xpPrele.Clone())
            End If
            xXmlTopNode.AppendChild(xCurNode)
            ConvertWP(xCurNode, xHtmlCode)

        End If
    End Sub
    ''' <summary>
    ''' 转换段落文本格式
    ''' </summary>
    ''' <param name="xXmlCurNode">需要往w:p节点插入数据（或者在其后新增w:p）的w:p节点</param>
    ''' <param name="xHtmlCode">html代码</param>
    ''' <remarks></remarks>

    Private Sub ConvertWP(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlNode)
        Dim sHtmlTagName As String = xHtmlCode.Name
        Dim xCurNode As XmlNode = xXmlCurNode
        Dim xXmlWPPr As XmlNode
        If CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:pPr").Count() <= 0 Then
            xXmlWPPr = xXmlDoc.CreateElement("w", "pPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") 'xXmlDoc.CreateElement("w:pPr")
            xXmlCurNode.InsertBefore(xXmlWPPr, xXmlCurNode.FirstChild())
        Else
            xXmlWPPr = CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:pPr")(0)
        End If

        '如果html是纯文本则直接插入数据
        If xHtmlCode.GetType().ToString() = "System.Xml.XmlText" Then
            Dim xXmlCurWR As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") 'xXmlDoc.CreateElement("w:r")
            xCurNode.AppendChild(xXmlCurWR)
            AppendTextToWr(xXmlCurWR, xHtmlCode.InnerText)
            Exit Sub
        End If

        Select Case sHtmlTagName
            '处理带有格式的文本数据
            Case "b"
                ConvertTextWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "u"
                ConvertTextWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "i"
                ConvertTextWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "strong"
                ConvertTextWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "span"
                ConvertTextWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "font"
                ConvertTextWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "img"
                Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                xCurNode.AppendChild(xNewWrNode)
                AppendImageToWr(xNewWrNode, xHtmlCode)
                Exit Sub
                ' 处理段落格式的数据                
            Case Else 'If sHtmlTagName = "p" Or sHtmlTagName = "div" Then
                '样式解析处理
                AppendWPStyle(xXmlWPPr, xHtmlCode)
                '递归处理段落内的子元素
                Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
                If ChildList Is Nothing Or ChildList.Count() <= 0 Then
                    Exit Sub
                ElseIf ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
                    Dim xXmlCurWR As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") 'xXmlDoc.CreateElement("w:r")
                    xCurNode.AppendChild(xXmlCurWR)
                    AppendTextToWr(xXmlCurWR, xHtmlCode.InnerText)
                    Exit Sub
                Else
                    Dim i As Integer = 0
                    Dim n As Integer = ChildList.Count()
                    While i < n
                        SelectWPByName(xXmlCurNode.ParentNode(), ChildList(i), xCurNode, CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:pPr")(0))
                        'SelectWPByName(xCurNode.ParentNode(), ChildList(i), xCurNode, CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:pPr")(0))
                        i += 1
                    End While
                End If
                Exit Sub
        End Select
    End Sub


    Private Sub ConvertTable(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlElement)
        Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
        Dim xCurNode As XmlNode = xXmlCurNode
        If CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:tblPr").Count() <= 0 Then
            xXmlCurNode.AppendChild(xXmlDoc.CreateElement("w", "tblPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
        End If
        AppendTBStyle(CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:tblPr")(0), xHtmlCode)
        If ChildList Is Nothing Or ChildList.Count() <= 0 Then
            Exit Sub
        End If
        For i As Integer = 0 To ChildList.Count() - 1
            If ChildList(i).ChildNodes().Count() > 0 Then
                xCurNode = xXmlDoc.CreateElement("w", "tr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                xXmlCurNode.AppendChild(xCurNode)
                AppendTableRow(xCurNode, ChildList(i))
            End If
        Next
    End Sub

    Private Sub AppendTableRow(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlElement)
        Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
        Dim xCurNode As XmlNode = xXmlCurNode
        If CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:trPr").Count() <= 0 Then
            xXmlCurNode.AppendChild(xXmlDoc.CreateElement("w", "trPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
        End If
        AppendTBStyle(CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:trPr")(0), xHtmlCode)
        If ChildList.Count() <= 0 Then
            Exit Sub
        End If
        For i As Integer = 0 To ChildList.Count() - 1
            xCurNode = xXmlDoc.CreateElement("w", "tc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            xXmlCurNode.AppendChild(xCurNode)
            AppendTableCell(xCurNode, ChildList(i))
        Next

    End Sub




    Private Sub AppendTableCell(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlElement)
        Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
        Dim xCurNode As XmlNode = xXmlCurNode ' xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        If CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:tcPr").Count() <= 0 Then
            xXmlCurNode.AppendChild(xXmlDoc.CreateElement("w", "tcPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
        End If
        AppendTBStyle(CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:tcPr")(0), xHtmlCode)
        If ChildList.Count() <= 0 Then
            Exit Sub
        End If
        If ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
            Dim xNewWPNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            Dim xNewWPrNode As XmlNode = xXmlDoc.CreateElement("w", "pPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            AppendWPStyle(xNewWPrNode, xHtmlCode.ParentNode().ParentNode())
            AppendWPStyle(xNewWPrNode, xHtmlCode.ParentNode())
            AppendWPStyle(xNewWPrNode, xHtmlCode)
            xNewWPNode.AppendChild(xNewWPrNode)
            If CType(xNewWPrNode, XmlElement).GetElementsByTagName("w:rPr").Count() > 0 Then
                xNewWrNode.AppendChild(CType(xNewWPrNode, XmlElement).GetElementsByTagName("w:rPr")(0).Clone())
            End If
            xNewWPNode.AppendChild(xNewWrNode)
            xXmlCurNode.AppendChild(xNewWPNode)
            AppendTextToWr(xNewWrNode, xHtmlCode.InnerText())
            Exit Sub
        End If

        xCurNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        Dim xWPrNode As XmlElement = xXmlDoc.CreateElement("w", "pPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        AppendWPStyle(xWPrNode, xHtmlCode.ParentNode().ParentNode()) '继承table标签的样式
        AppendWPStyle(xWPrNode, xHtmlCode.ParentNode()) '继承tr标签的样式
        AppendWPStyle(xWPrNode, xHtmlCode) '插入当前td标签的样式
        xCurNode.AppendChild(xWPrNode)
        For i As Integer = 0 To ChildList.Count() - 1
            SelectWPByName(xXmlCurNode, ChildList(i), xCurNode, xWPrNode)
            'SelectWPByName(xCurNode.ParentNode(), ChildList(i), xCurNode, xWPrNode)
        Next

    End Sub

    ''' <summary>
    ''' 转换文本格式，其中xXmlCurNode是w:p或者w:tbl节点
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="xHtmlCode">需要转换的html代码</param>
    ''' <remarks></remarks>
    Private Sub ConvertTextWr(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlNode)
        Dim sHtmlTagName As String = xHtmlCode.Name
        Dim xCurNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        Dim xCurWrPr As XmlNode '= xXmlDoc.CreateElement("w", "rPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        If CType(CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr").Count() <= 0 Then
            xCurWrPr = xXmlDoc.CreateElement("w", "rPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        Else
            xCurWrPr = CType(CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr")(0).Clone()
        End If
        xCurNode.AppendChild(xCurWrPr)
        xXmlCurNode.AppendChild(xCurNode)
        AppendWPStyle(xCurWrPr, xHtmlCode)
        If xHtmlCode.GetType().ToString() = "System.Xml.XmlText" Then

            AppendTextToWr(xCurNode, xHtmlCode.InnerText)
            Exit Sub
        End If
        Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
        If ChildList Is Nothing Or ChildList.Count() <= 0 Then
            Exit Sub
        End If

        Select Case sHtmlTagName
            Case "b"
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "u"
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "i"
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "strong"
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub

            Case "font"
                CType(xHtmlCode, XmlElement).SetAttribute("style", "color:" & CType(xHtmlCode, XmlElement).GetAttribute("color") & ";" & CType(xHtmlCode, XmlElement).GetAttribute("style"))
                AppendWPStyle(xCurWrPr, xHtmlCode)
                If ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
                    AppendTextToWr(xCurNode, ChildList(0).InnerText)
                Else
                    For i As Integer = 0 To ChildList.Count() - 1
                        'SelectWrByName(xXmlCurNode, ChildList(i), xCurNode, xCurWrPr)
                        SelectWrByName(xCurNode.ParentNode(), ChildList(i), xCurNode, xCurWrPr)
                    Next
                End If
            Case "img"
                Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                xXmlCurNode.AppendChild(xNewWrNode)
                AppendImageToWr(xNewWrNode, xHtmlCode)
                Exit Sub
            Case Else '"span"
                If ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
                    AppendTextToWr(xCurNode, ChildList(0).InnerText)
                Else
                    For i As Integer = 0 To ChildList.Count() - 1
                        'SelectWrByName(xXmlCurNode, ChildList(i), xCurNode, xCurWrPr)
                        SelectWrByName(xCurNode.ParentNode(), ChildList(i), xCurNode, xCurWrPr)
                    Next
                End If
        End Select

    End Sub

    ''' <summary>
    ''' 插入数据行选择
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="xHtmlCode"></param>
    ''' <param name="xCurNode"></param>
    ''' <param name="xrPrele"></param>
    ''' <remarks></remarks>
    Private Sub SelectWrByName(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlNode, ByRef xCurNode As XmlNode, ByVal xrPrele As XmlElement)
        Dim sHtmlTagName As String = xHtmlCode.Name
        If xHtmlCode.GetType().ToString() = "System.Xml.XmlText" Then
            Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            If Not xrPrele Is Nothing Then
                xNewWrNode.AppendChild(xrPrele.Clone())
            End If
            xXmlCurNode.AppendChild(xNewWrNode)
            xCurNode = xNewWrNode
            AppendTextToWr(xCurNode, xHtmlCode.InnerText)
            Exit Sub
        End If
        Select Case sHtmlTagName
            Case "b"
                Dim xNewWrNode As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                AppendWPStyle(xNewWrNode.GetElementsByTagName("w:rPr")(0), xHtmlCode)
                xXmlCurNode.AppendChild(xNewWrNode)
                xCurNode = xNewWrNode
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "u"
                Dim xNewWrNode As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                AppendWPStyle(xNewWrNode.GetElementsByTagName("w:rPr")(0), xHtmlCode)
                xXmlCurNode.AppendChild(xNewWrNode)
                xCurNode = xNewWrNode
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "i"
                Dim xNewWrNode As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                AppendWPStyle(xNewWrNode.GetElementsByTagName("w:rPr")(0), xHtmlCode)
                xXmlCurNode.AppendChild(xNewWrNode)
                xCurNode = xNewWrNode
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub
            Case "strong"
                Dim xNewWrNode As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                AppendWPStyle(xNewWrNode.GetElementsByTagName("w:rPr")(0), xHtmlCode)
                xXmlCurNode.AppendChild(xNewWrNode)
                xCurNode = xNewWrNode
                AppendXToWr(xCurNode, xHtmlCode)
                Exit Sub

            Case "font"
                Dim xNewWrNode As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                CType(xHtmlCode, XmlElement).SetAttribute("style", "color:" & CType(xHtmlCode, XmlElement).GetAttribute("color") & ";" & CType(xHtmlCode, XmlElement).GetAttribute("style"))
                AppendWPStyle(xNewWrNode.GetElementsByTagName("w:rPr")(0), xHtmlCode)
                xXmlCurNode.AppendChild(xNewWrNode)
                xCurNode = xNewWrNode
                Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
                If ChildList.Count() <= 0 Then
                    Exit Sub
                End If
                If ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
                    AppendTextToWr(xCurNode, ChildList(0).InnerText)
                Else
                    For i As Integer = 0 To ChildList.Count() - 1
                        'SelectWrByName(xXmlCurNode, ChildList(i), xCurNode, xNewWrNode.GetElementsByTagName("w:rPr")(0))
                        SelectWrByName(xCurNode.ParentNode(), ChildList(i), xCurNode, xNewWrNode.GetElementsByTagName("w:rPr")(0))
                    Next
                End If
                Exit Sub
            Case "img"
                AppendImageToWr(xXmlCurNode, xHtmlCode)
                Exit Sub
            Case "br"
                Dim xNewWpNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                xNewWpNode.AppendChild(xNewWrNode)
                xXmlCurNode.ParentNode().AppendChild(xNewWpNode)
                xCurNode = xNewWrNode
                Exit Sub
            Case "div"
                Dim xNewWpNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                xNewWpNode.AppendChild(xNewWrNode)
                xXmlCurNode.ParentNode().AppendChild(xNewWpNode)
                xCurNode = xNewWrNode
                ConvertWP(xNewWpNode, xHtmlCode)
                Exit Sub
            Case "p"
                Dim xNewWpNode As XmlNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                Dim xNewWrNode As XmlNode = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                xNewWpNode.AppendChild(xNewWrNode)
                xXmlCurNode.ParentNode().AppendChild(xNewWpNode)
                xCurNode = xNewWrNode
                ConvertWP(xNewWpNode, xHtmlCode)
                Exit Sub
            Case "table"
                Dim xNewWpNode As XmlNode = xXmlDoc.CreateElement("w", "tbl", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                '调用样式拷贝,还没有实现
                xXmlCurNode.ParentNode().AppendChild(xNewWpNode)
                ConvertTable(xNewWpNode, xHtmlCode)
                Exit Sub
            Case Else '"span"
                Dim xNewWrNode As XmlElement = xXmlDoc.CreateElement("w", "r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                If Not xrPrele Is Nothing Then
                    xNewWrNode.AppendChild(xrPrele.Clone())
                End If
                AppendWPStyle(xNewWrNode.GetElementsByTagName("w:rPr")(0), xHtmlCode)
                xXmlCurNode.AppendChild(xNewWrNode)
                xCurNode = xNewWrNode
                Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
                If ChildList.Count() <= 0 Then
                    Exit Sub
                End If
                If ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
                    AppendTextToWr(xCurNode, ChildList(0).InnerText)
                Else
                    For i As Integer = 0 To ChildList.Count() - 1
                        'SelectWrByName(xXmlCurNode, ChildList(i), xCurNode, xNewWrNode.GetElementsByTagName("w:rPr")(0))
                        SelectWrByName(xCurNode.ParentNode(), ChildList(i), xCurNode, xNewWrNode.GetElementsByTagName("w:rPr")(0))
                    Next
                End If
                Exit Sub
        End Select
    End Sub
    ''' <summary>
    ''' 往w:r节点添加文本
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="sHtmlText"></param>
    ''' <remarks></remarks>
    Public Sub AppendTextToWr(ByVal xXmlCurNode As XmlNode, ByVal sHtmlText As String)
        Dim xXmlTextNode As XmlElement = xXmlDoc.CreateElement("w", "t", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        If CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:rPr").Count() <= 0 Then
            Dim xCurWrPr As XmlNode
            If CType(CType(xXmlCurNode.ParentNode(), XmlElement).GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr").Count() <= 0 Then
                xCurWrPr = xXmlDoc.CreateElement("w", "rPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            Else
                xCurWrPr = CType(CType(xXmlCurNode.ParentNode(), XmlElement).GetElementsByTagName("w:pPr")(0), XmlElement).GetElementsByTagName("w:rPr")(0).Clone()
            End If
            xXmlCurNode.AppendChild(xCurWrPr)
        End If

        xXmlCurNode.AppendChild(xXmlTextNode)
        Dim xAttrib As XmlAttribute = xXmlDoc.CreateAttribute("xml:space")
        xAttrib.Value = "preserve"
        xXmlTextNode.SetAttributeNode(xAttrib)
        xXmlTextNode.InnerText = sHtmlText.Replace("&nbsp;", " ")
    End Sub

    ''' <summary>
    ''' 往特定位置插入图片
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="xHtmlCode"></param>
    ''' <remarks></remarks>
    Private Sub AppendImageToWr(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlElement)
        Dim sFileData As String = ""
        Dim sFileName As String = ""
        If xHtmlCode.GetAttribute("src").Trim().IndexOf("http") < 0 Then
            sFileName = xHtmlCode.GetAttribute("src") 'xHtmlCode.OuterXml.Substring(xHtmlCode.OuterXml.IndexOf("src"))
            sFileName = System.Web.HttpContext.Current.Server.MapPath("~") & sFileName.Replace("/", "\")
            sFileName = sFileName.Replace("\\", "\")
            sFileData = ToBase64(sFileName)
        Else
            Dim sFileUrl As String = xHtmlCode.GetAttribute("src")
            Dim sExtensionName As String = MyString.RightOfLast(sFileUrl, ".")
            Dim sPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache")
            Dim sNewFileName As String = Path.Combine(sPath, Guid.NewGuid().ToString("N") + "." + sExtensionName)
            MyWeb.LoadImageFromUrl(sFileUrl, sNewFileName)
            sFileData = ToBase64(sNewFileName)
            sFileName = sNewFileName
        End If


        '图片的宽度、高度
        Dim iImageWidthHeight As Integer() = MyImage.GetImageWidthHeight(sFileName)

        Dim sRid As String = DateTime.Now().Day.ToString() & DateTime.Now().Hour.ToString() & DateTime.Now().Minute().ToString() & DateTime.Now().Second.ToString()

        '存放图片数据节点
        Dim xPkgFileData As XmlElement = xXmlDoc.CreateElement("pkg", "part", "http://schemas.microsoft.com/office/2006/xmlPackage")
        Dim xNewAttr As XmlAttribute = xXmlDoc.CreateAttribute("pkg", "contentType", "http://schemas.microsoft.com/office/2006/xmlPackage")
        xNewAttr.Value = "image/jpeg"
        xPkgFileData.SetAttributeNode(xNewAttr)
        Dim sPkgRefTagName As String = "media/image" & sRid & ".jpeg"
        xNewAttr = xXmlDoc.CreateAttribute("pkg", "name", "http://schemas.microsoft.com/office/2006/xmlPackage")
        xNewAttr.Value = "/word/" & sPkgRefTagName
        xPkgFileData.SetAttributeNode(xNewAttr)
        xNewAttr = xXmlDoc.CreateAttribute("pkg", "compression", "http://schemas.microsoft.com/office/2006/xmlPackage")
        xNewAttr.Value = "store"
        xPkgFileData.SetAttributeNode(xNewAttr)
        xPkgFileData.AppendChild(xXmlDoc.CreateElement("pkg", "binaryData", "http://schemas.microsoft.com/office/2006/xmlPackage"))
        xPkgFileData.ChildNodes()(0).InnerText = sFileData
        xRootNode.AppendChild(xPkgFileData)
        'body中存放图片的位置节点
        Dim xPic As XmlElement = xXmlDoc.CreateElement("w", "pict", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        Dim xShape As XmlElement = xXmlDoc.CreateElement("v", "shape", "urn:schemas-microsoft-com:vml")
        AddAttrib(xShape, "", "style", "width:" & (iImageWidthHeight(0) * 3 / 4) & "pt;" & "height:" & (iImageWidthHeight(1) * 3 / 4) & "pt")
        Dim xImageData As XmlElement = xXmlDoc.CreateElement("v", "imagedata", "urn:schemas-microsoft-com:vml")

        xNewAttr = xXmlDoc.CreateAttribute("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")
        xNewAttr.Value = "rId" & sRid
        xImageData.SetAttributeNode(xNewAttr)
        xShape.AppendChild(xImageData)
        xPic.AppendChild(xShape)
        xXmlCurNode.AppendChild(xPic)

        '添加映射节点
        Dim xPkgRef As XmlElement = GetEleByPkgName(xRootNode, "/word/_rels/document.xml.rels")
        Dim xRelation As XmlElement = xXmlDoc.CreateElement("Relationship")
        xNewAttr = xXmlDoc.CreateAttribute("Id")
        xNewAttr.Value = "rId" & sRid
        xRelation.SetAttributeNode(xNewAttr)
        xNewAttr = xXmlDoc.CreateAttribute("Type")
        xNewAttr.Value = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image"
        xRelation.SetAttributeNode(xNewAttr)
        xNewAttr = xXmlDoc.CreateAttribute("Target")
        xNewAttr.Value = sPkgRefTagName
        xRelation.SetAttributeNode(xNewAttr)

        xPkgRef.ChildNodes()(0).ChildNodes()(0).AppendChild(xRelation)
    End Sub
    ''' <summary>
    ''' 往w:r中添加b,u,i,strong等修饰的样式
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="xHtmlCode"></param>
    ''' <remarks></remarks>
    Private Sub AppendXToWr(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlNode)
        Dim xCurNode As XmlNode = xXmlCurNode
        Dim sHtmlTagName As String = xHtmlCode.Name
        sHtmlTagName = IIf(sHtmlTagName.Equals("strong"), "b", sHtmlTagName)


        Dim ChildList As XmlNodeList = xHtmlCode.ChildNodes()
        If ChildList Is Nothing Or ChildList.Count() <= 0 Then
            Exit Sub
        End If
        If CType(xCurNode, XmlElement).GetElementsByTagName("w:rPr").Count() <= 0 Then
            xCurNode.InsertBefore(xXmlDoc.CreateElement("w", "rPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"), xCurNode.FirstChild())
        End If

        Dim xNewEle As XmlElement = xXmlDoc.CreateElement("w", sHtmlTagName, "http://schemas.openxmlformats.org/wordprocessingml/2006/main") 'xXmlDoc.CreateElement("w:" & sHtmlTagName)
        If sHtmlTagName = "u" Then
            Dim xNewAttr As XmlAttribute = xXmlDoc.CreateAttribute("w", "val", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") 'xXmlDoc.CreateAttribute("w:val")
            xNewAttr.Value = "single"
            xNewEle.SetAttributeNode(xNewAttr)
        End If
        CType(xCurNode, XmlElement).GetElementsByTagName("w:rPr")(0).AppendChild(xNewEle)
        If ChildList.Count() = 1 And ChildList(0).GetType().ToString() = "System.Xml.XmlText" Then
            AppendTextToWr(xCurNode, xHtmlCode.InnerText)
            Exit Sub
        Else

            Dim i As Integer = 0
            Dim n As Integer = ChildList.Count()
            While i < n
                SelectWrByName(xCurNode.ParentNode(), ChildList(i), xCurNode, CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:rPr")(0))
                i += 1
            End While
        End If
    End Sub

    Private Sub AppendWPStyle(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlElement)
        If xHtmlCode.Attributes().Count() <= 0 Then
            Exit Sub
        End If
        Dim xCurNode As XmlElement = xXmlCurNode
        Dim sStyle As String = xHtmlCode.GetAttribute("style")
        'Dim xNewAttr As XmlAttribute
        If xCurNode.Name = "w:pPr" Then
            '先插入段落相关样式
            Dim sAttribValue = xHtmlCode.GetAttribute("align")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If CType(xCurNode, XmlElement).GetElementsByTagName("w:jc").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "jc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:jc")(0), "w", "val", sAttribValue)
            End If
            '
            If xCurNode.GetElementsByTagName("w:rPr").Count() <= 0 Then
                xXmlCurNode.AppendChild(xXmlDoc.CreateElement("w", "rPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
            End If
            xCurNode = CType(xXmlCurNode, XmlElement).GetElementsByTagName("w:rPr")(0)
        End If

        Dim aStyleList As String() = sStyle.Split(";")
        For i As Integer = 0 To aStyleList.Length - 1
            If String.IsNullOrEmpty(aStyleList(i)) Then
                Continue For
            End If
            Dim sAttrName As String = ""
            Dim sAttrValue As String = ""
            Dim aTempStr() As String = aStyleList(i).Split(":")
            sAttrName = aTempStr(0).Trim()
            If aTempStr.Length >= 2 Then
                sAttrValue = aTempStr(1).Trim()
            Else
                sAttrValue = ""
            End If

            Select Case sAttrName
                Case "font-family"
                    If xCurNode.GetElementsByTagName("w:rFonts").Count() <= 0 Then
                        xCurNode.AppendChild(xXmlDoc.CreateElement("w", "rFonts", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                    End If
                    AddAttrib(xCurNode.GetElementsByTagName("w:rFonts")(0), "w", "cs", sAttrValue)
                    AddAttrib(xCurNode.GetElementsByTagName("w:rFonts")(0), "w", "hAnsi", sAttrValue)
                    AddAttrib(xCurNode.GetElementsByTagName("w:rFonts")(0), "w", "ascii", sAttrValue)
                    AddAttrib(xCurNode.GetElementsByTagName("w:rFonts")(0), "w", "eastAsia", sAttrValue)
                Case "color"
                    If xCurNode.GetElementsByTagName("w:color").Count() <= 0 Then
                        xCurNode.AppendChild(xXmlDoc.CreateElement("w", "color", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                    End If
                    AddAttrib(xCurNode.GetElementsByTagName("w:color")(0), "w", "val", sAttrValue)
                Case "font-size"
                    If xCurNode.GetElementsByTagName("w:sz").Count() <= 0 Then
                        xCurNode.AppendChild(xXmlDoc.CreateElement("w", "sz", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                    End If
                    AddAttrib(xCurNode.GetElementsByTagName("w:sz")(0), "w", "val", Convert.ToDouble(GetLengthAsDxa(sAttrValue, (567).ToString())) / 567 * 28)

                    If xCurNode.GetElementsByTagName("w:szCs").Count() <= 0 Then
                        xCurNode.AppendChild(xXmlDoc.CreateElement("w", "szCs", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                    End If
                    AddAttrib(xCurNode.GetElementsByTagName("w:szCs")(0), "w", "val", Convert.ToDouble(GetLengthAsDxa(sAttrValue, (567).ToString())) / 567 * 28)
            End Select
        Next
    End Sub

    Private Sub AppendTBStyle(ByVal xXmlCurNode As XmlNode, ByVal xHtmlCode As XmlElement)
        If xHtmlCode.Attributes().Count() <= 0 Then
            Exit Sub
        End If
        Dim xCurNode As XmlElement = xXmlCurNode

        '插入table的全局样式
        If xCurNode.Name = "w:tblPr" Then 'begin w:tblPr
            'align属性
            Dim sAttribValue As String = xHtmlCode.GetAttribute("align")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:jc").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "jc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:jc")(0), "w", "val", sAttribValue)
            End If

            'cellspacing
            sAttribValue = xHtmlCode.GetAttribute("cellspacing")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:tblCellSpacing").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tblCellSpacing", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:tblCellSpacing")(0), "w", "w", Convert.ToDouble(sAttribValue) * 567 / 28)
                AddAttrib(xCurNode.GetElementsByTagName("w:tblCellSpacing")(0), "w", "type", "dxa")
            End If

            'width
            sAttribValue = xHtmlCode.GetAttribute("width")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:tblW").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tblW", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:tblW")(0), "w", "w", GetLengthAsDxa(sAttribValue, (18 * 567).ToString()))
                AddAttrib(xCurNode.GetElementsByTagName("w:tblW")(0), "w", "type", "dxa")
            End If

            'begin style
            sAttribValue = xHtmlCode.GetAttribute("style").Trim()
            If Not String.IsNullOrEmpty(sAttribValue) Then
                Dim aAttribs As String() = sAttribValue.Split(";")
                For i As Integer = 0 To aAttribs.Length - 1
                    If String.IsNullOrEmpty(aAttribs(i)) Then
                        Continue For
                    End If
                    Dim sAttrName As String = ""
                    'Dim sAttrValue As String = ""
                    Dim aAttr = aAttribs(i).Split(":")
                    sAttrName = aAttr(0).Trim()
                    If (aAttr.Length >= 2) Then
                        sAttribValue = aAttr(1)
                    Else
                        sAttribValue = ""
                    End If

                    If sAttrName = "border" Then
                        If xCurNode.GetElementsByTagName("w:tblBorders").Count() <= 0 Then
                            xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tblBorders", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        End If
                        If String.IsNullOrEmpty(xCurNode.GetElementsByTagName("w:tblBorders")(0).InnerXml) Then
                            xCurNode.GetElementsByTagName("w:tblBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "top", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.GetElementsByTagName("w:tblBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "left", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.GetElementsByTagName("w:tblBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "bottom", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.GetElementsByTagName("w:tblBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "right", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        End If

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "sz", "4")

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "sz", "4")

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "sz", "4")

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tblBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "sz", "4")
                    End If
                    If sAttrName = "width" Then
                        If Not String.IsNullOrEmpty(sAttribValue) Then
                            If xCurNode.GetElementsByTagName("w:tblW").Count() <= 0 Then
                                xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tblW", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            End If
                            AddAttrib(xCurNode.GetElementsByTagName("w:tblW")(0), "w", "w", GetLengthAsDxa(sAttribValue, (18 * 567).ToString()))
                            AddAttrib(xCurNode.GetElementsByTagName("w:tblW")(0), "w", "type", "dxa")
                        End If
                    End If
                Next
            End If 'end style

        End If 'end w:tblPr

        '插入table的行样式
        If xCurNode.Name = "w:trPr" Then 'begin w:trPr
            'align属性
            Dim sAttribValue As String = xHtmlCode.GetAttribute("align")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:jc").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "jc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:jc")(0), "w", "val", sAttribValue)
            End If

            'cellspacing
            If CType(xCurNode.ParentNode(), XmlElement).GetElementsByTagName("w:tblPr").Count() > 0 Then
                If CType(CType(xCurNode.ParentNode(), XmlElement).GetElementsByTagName("w:tblPr")(0), XmlElement).GetElementsByTagName("w:tblCellSpacing").Count() > 0 Then
                    xCurNode.AppendChild(CType(CType(xCurNode.ParentNode(), XmlElement).GetElementsByTagName("w:tblPr")(0), XmlElement).GetElementsByTagName("w:tblCellSpacing")(0).Clone())
                End If
            End If

        End If 'end w:trPr

        '插入table的td单元格样式
        If xCurNode.Name = "w:tcPr" Then 'begin w:tcPr
            'align属性
            Dim sAttribValue As String = xHtmlCode.GetAttribute("align")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:vAlign").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "vAlign", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:vAlign")(0), "w", "val", sAttribValue)
            End If

            'width属性
            sAttribValue = xHtmlCode.GetAttribute("width")
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:tcW").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tcW", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:tcW")(0), "w", "w", Convert.ToDouble(GetLengthAsDxa(sAttribValue, (18 * 567).ToString())) / 2.0412)
                AddAttrib(xCurNode.GetElementsByTagName("w:tcW")(0), "w", "type", "pct")
            End If

            'colspan
            sAttribValue = xHtmlCode.GetAttribute("colspan").Trim()
            If Not String.IsNullOrEmpty(sAttribValue) Then
                If xCurNode.GetElementsByTagName("w:gridSpan").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "gridSpan", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                AddAttrib(xCurNode.GetElementsByTagName("w:gridSpan")(0), "w", "val", sAttribValue)
            End If

            'rowspan处理较为复杂
            sAttribValue = xHtmlCode.GetAttribute("rowspan").Trim()
            If Not String.IsNullOrEmpty(sAttribValue) Then
                Dim iRowSpanValue As Integer = 1
                Try
                    iRowSpanValue = Convert.ToInt32(sAttribValue.Trim())
                Catch ex As Exception
                    iRowSpanValue = 1
                End Try

                If xCurNode.GetElementsByTagName("w:vMerge").Count() <= 0 Then
                    xCurNode.AppendChild(xXmlDoc.CreateElement("w", "vMerge", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                End If
                Dim xHtmlCurTd As XmlElement = xHtmlCode
                Dim xHtmlCurTr As XmlElement = xHtmlCurTd.ParentNode
                Dim iTdIndex As Integer = 0
                Dim xHtmlCurTrChilds As XmlNodeList = xHtmlCurTr.ChildNodes()
                For i As Integer = 0 To xHtmlCurTrChilds.Count() - 1
                    If xHtmlCurTd.Equals(xHtmlCurTrChilds(i)) Then
                        iTdIndex = i
                        Exit For
                    End If
                Next
                For i As Integer = 1 To iRowSpanValue - 1
                    If Not xHtmlCurTr.NextSibling() Is Nothing Then
                        xHtmlCurTr = xHtmlCurTr.NextSibling()
                        xHtmlCurTd = xHtmlCurTr.LastChild()
                        If Not xHtmlCurTd Is Nothing Then
                            xHtmlCurTd.SetAttribute("rowspanindex", xHtmlCurTd.GetAttribute("rowspanindex") & iTdIndex & ";")
                        End If

                    End If
                Next
                AddAttrib(xCurNode.GetElementsByTagName("w:vMerge")(0), "w", "val", "restart")
            End If 'end rowspan

            'rowspanindex
            sAttribValue = xHtmlCode.GetAttribute("rowspanindex").Trim()
            If Not String.IsNullOrEmpty(sAttribValue) Then
                Dim aRowSpans As String() = sAttribValue.Split(";")
                Dim xHtmlCurTd As XmlElement = xHtmlCode
                Dim xHtmlCurTr As XmlElement = xHtmlCurTd.ParentNode
                For i As Integer = 0 To aRowSpans.Length - 1
                    If String.IsNullOrEmpty(aRowSpans(i).Trim()) Then
                        Continue For
                    End If
                    If Not xCurNode.ParentNode().ParentNode().ChildNodes()(Convert.ToInt32(aRowSpans(i).Trim()) + 1) Is Nothing Then
                        Dim xNewTc As XmlElement = xXmlDoc.CreateElement("w", "tc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                        'xNewTc.AppendChild(xXmlDoc.CreateElement("w", "tcPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        Dim xTempTcPr As XmlElement = CType(xCurNode.ParentNode().ParentNode().PreviousSibling().ChildNodes()(Convert.ToInt32(aRowSpans(i).Trim()) + 1), XmlElement).GetElementsByTagName("w:tcPr")(0).Clone()
                        xTempTcPr.RemoveChild(xTempTcPr.GetElementsByTagName("w:vMerge")(0))
                        xNewTc.AppendChild(xTempTcPr)
                        xNewTc.GetElementsByTagName("w:tcPr")(0).AppendChild(xXmlDoc.CreateElement("w", "vMerge", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        xNewTc.AppendChild(xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        Dim iToInsertIndex As Integer = Convert.ToInt32(aRowSpans(i).Trim()) + 1
                        'Dim xToInsertBefore As XmlElement = xCurNode.ParentNode().ParentNode().ChildNodes()(iToInsertIndex)
                        xCurNode.ParentNode().ParentNode().InsertBefore(xNewTc, xCurNode.ParentNode().ParentNode().ChildNodes()(iToInsertIndex))
                    Else
                        Dim xNewTc As XmlElement
                        Dim j As Integer = 0
                        For j = xCurNode.ParentNode().ParentNode().ChildNodes().Count() - 1 To Convert.ToInt32(aRowSpans(i).Trim()) - 1
                            xNewTc = xXmlDoc.CreateElement("w", "tc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                            xNewTc.AppendChild(xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.ParentNode().ParentNode().AppendChild(xNewTc)
                        Next
                        xNewTc = xXmlDoc.CreateElement("w", "tc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
                        'xNewTc.AppendChild(xXmlDoc.CreateElement("w", "tcPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        Dim xTempTcPr As XmlElement = CType(xCurNode.ParentNode().ParentNode().PreviousSibling().ChildNodes()(j + 1), XmlElement).GetElementsByTagName("w:tcPr")(0).Clone()
                        xTempTcPr.RemoveChild(xTempTcPr.GetElementsByTagName("w:vMerge")(0))
                        xNewTc.AppendChild(xTempTcPr)
                        xNewTc.GetElementsByTagName("w:tcPr")(0).AppendChild(xXmlDoc.CreateElement("w", "vMerge", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        xNewTc.AppendChild(xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        xCurNode.ParentNode().ParentNode().AppendChild(xNewTc)
                    End If
                Next

            End If

            'style
            sAttribValue = xHtmlCode.GetAttribute("style").Trim()
            If Not String.IsNullOrEmpty(sAttribValue) Then
                Dim aAttribs As String() = sAttribValue.Split(";")
                For i As Integer = 0 To aAttribs.Length - 1
                    If String.IsNullOrEmpty(aAttribs(i)) Then
                        Continue For
                    End If
                    Dim sAttrName As String = ""
                    Dim aAttr = aAttribs(i).Split(":")
                    sAttrName = aAttr(0).Trim()
                    If (aAttr.Length >= 2) Then
                        sAttribValue = aAttr(1)
                    Else
                        sAttribValue = ""
                    End If
                    If sAttrName = "width" Then
                        If Not String.IsNullOrEmpty(sAttribValue) Then
                            If xCurNode.GetElementsByTagName("w:tcW").Count() <= 0 Then
                                xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tcW", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            End If
                            AddAttrib(xCurNode.GetElementsByTagName("w:tcW")(0), "w", "w", Convert.ToDouble(GetLengthAsDxa(sAttribValue, (18 * 567).ToString())) / 2.0412)
                            AddAttrib(xCurNode.GetElementsByTagName("w:tcW")(0), "w", "type", "pct")
                        End If
                    End If
                    If sAttrName = "border" Then
                        If xCurNode.GetElementsByTagName("w:tcBorders").Count() <= 0 Then
                            xCurNode.AppendChild(xXmlDoc.CreateElement("w", "tcBorders", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        End If
                        If String.IsNullOrEmpty(xCurNode.GetElementsByTagName("w:tcBorders")(0).InnerXml) Then
                            xCurNode.GetElementsByTagName("w:tcBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "top", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.GetElementsByTagName("w:tcBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "left", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.GetElementsByTagName("w:tcBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "bottom", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                            xCurNode.GetElementsByTagName("w:tcBorders")(0).AppendChild(xXmlDoc.CreateElement("w", "right", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"))
                        End If

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:top")(0), "w", "sz", "4")

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:left")(0), "w", "sz", "4")

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:bottom")(0), "w", "sz", "4")

                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "val", "single")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "space", "0")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "color", "000000")
                        AddAttrib(CType(xCurNode.GetElementsByTagName("w:tcBorders")(0), XmlElement).GetElementsByTagName("w:right")(0), "w", "sz", "4")
                    End If

                Next

            End If 'end style

        End If 'end w:tcPr

    End Sub

    ''' <summary>
    ''' 给属性赋值
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="sNamespace"></param>
    ''' <param name="sAttrName"></param>
    ''' <param name="sAttrValue"></param>
    ''' <remarks></remarks>
    Public Sub AddAttrib(ByVal xXmlCurNode As XmlElement, ByVal sNamespace As String, ByVal sAttrName As String, ByVal sAttrValue As String)
        Dim xNewAttr As XmlAttribute
        Dim sNamespaceUrl As String = ""
        Select Case sNamespace
            Case "w"
                sNamespaceUrl = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"
            Case "v"
                sNamespaceUrl = "urn:schemas-microsoft-com:vml"
            Case "pkg"
                sNamespaceUrl = "http://schemas.microsoft.com/office/2006/xmlPackage"
            Case "wp"
                sNamespaceUrl = "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"
            Case "wne"
                sNamespaceUrl = "http://schemas.microsoft.com/office/word/2006/wordml"
            Case "w10"
                sNamespaceUrl = "urn:schemas-microsoft-com:office:word"
            Case "m"
                sNamespaceUrl = "http://schemas.openxmlformats.org/officeDocument/2006/math"
            Case "r"
                sNamespaceUrl = "http://schemas.openxmlformats.org/officeDocument/2006/relationships"
            Case "o"
                sNamespaceUrl = "urn:schemas-microsoft-com:office:office"
            Case "ve"
                sNamespaceUrl = "http://schemas.openxmlformats.org/markup-compatibility/2006"
        End Select
        If Not (xXmlCurNode.HasAttribute(sAttrName, sNamespaceUrl)) Then
            xNewAttr = xXmlDoc.CreateAttribute(sNamespace, sAttrName, sNamespaceUrl)
            xXmlCurNode.SetAttributeNode(xNewAttr)
        End If
        'xXmlCurNode.SetAttribute(sNamespace & ":" & sAttrName, sAttrValue)
        xXmlCurNode.SetAttribute(sAttrName, sNamespaceUrl, sAttrValue)
    End Sub

    ''' <summary>
    ''' 给属性赋值
    ''' </summary>
    ''' <param name="xXmlCurNode"></param>
    ''' <param name="sNamespace"></param>
    ''' <param name="sAttrName"></param>
    ''' <param name="sAttrValue"></param>
    ''' <param name="xXmlCurDoc"></param>
    ''' <remarks></remarks>
    Public Sub AddAttribEX(ByVal xXmlCurNode As XmlElement, ByVal sNamespace As String, ByVal sAttrName As String, ByVal sAttrValue As String, ByVal xXmlCurDoc As XmlDocument)
        Dim xNewAttr As XmlAttribute
        Dim sNamespaceUrl As String = ""
        Select Case sNamespace
            Case "w"
                sNamespaceUrl = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"
            Case "v"
                sNamespaceUrl = "urn:schemas-microsoft-com:vml"
            Case "pkg"
                sNamespaceUrl = "http://schemas.microsoft.com/office/2006/xmlPackage"
            Case "wp"
                sNamespaceUrl = "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"
            Case "wne"
                sNamespaceUrl = "http://schemas.microsoft.com/office/word/2006/wordml"
            Case "w10"
                sNamespaceUrl = "urn:schemas-microsoft-com:office:word"
            Case "m"
                sNamespaceUrl = "http://schemas.openxmlformats.org/officeDocument/2006/math"
            Case "r"
                sNamespaceUrl = "http://schemas.openxmlformats.org/officeDocument/2006/relationships"
            Case "o"
                sNamespaceUrl = "urn:schemas-microsoft-com:office:office"
            Case "ve"
                sNamespaceUrl = "http://schemas.openxmlformats.org/markup-compatibility/2006"
        End Select

        If Not (xXmlCurNode.HasAttribute(sAttrName, sNamespaceUrl)) Then
            xNewAttr = xXmlCurDoc.CreateAttribute(sNamespace, sAttrName, sNamespaceUrl)
            xXmlCurNode.SetAttributeNode(xNewAttr)
        End If
        xXmlCurNode.SetAttribute(sAttrName, sNamespaceUrl, sAttrValue)
    End Sub


    ''' <summary>
    ''' 转换html中的尺寸为word中的尺寸。转换对应关系：567 dxa/cm;28px/cm;2.0412dxa/pct;2.54cm/in;72pt/in;12pt/pc;A4纸张尺寸：21cm*29.7cm
    ''' </summary>
    ''' <param name="sHtmlLength"></param>
    ''' <param name="sDefLength"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLengthAsDxa(ByVal sHtmlLength As String, ByVal sDefLength As String) As String
        If String.IsNullOrEmpty(sHtmlLength) Then
            Return ""
        End If
        If String.IsNullOrEmpty(sDefLength) Then
            sDefLength = "0"
        End If
        Dim sResult As String = ""
        Try
            If sHtmlLength.IndexOf("%") >= 0 Then
                sResult = sHtmlLength.Replace("%", "").Trim()
                sResult = (21 * Convert.ToDouble(sResult) / 100).ToString()
            Else
                Dim sDW As String = sHtmlLength.Substring(sHtmlLength.Length() - 2, 2)
                sResult = sHtmlLength.Substring(0, sHtmlLength.Length() - 2).Trim()
                Select Case sDW
                    Case "mm"
                        sResult = (Convert.ToDouble(sResult) / 10).ToString()
                    Case "px"
                        sResult = (Convert.ToDouble(sResult) / 28).ToString()
                    Case "in"
                        sResult = (Convert.ToDouble(sResult) * 2.54).ToString()
                    Case "pt"
                        sResult = (Convert.ToDouble(sResult) * 2.54 / 72).ToString()
                    Case "pc"
                        sResult = (Convert.ToDouble(sResult) * 2.54 * 12 / 72).ToString()
                    Case "em"
                        sResult = (Convert.ToDouble(sResult) * 16 * 2.54 / 72).ToString()
                End Select
            End If
            sResult = Convert.ToDouble(sResult) * 567
        Catch ex As Exception
            sResult = sDefLength
        End Try
        Return sResult
    End Function

    ''' <summary>
    ''' 合并两个word文档
    ''' </summary>
    ''' <param name="xXmlCurNode">指定的位置节点（传入nothing，把文档新增到最后）</param>
    ''' <param name="sOtherFilePath">word文档的路径</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddWordDocAtWP(ByVal xXmlCurNode As XmlElement, ByVal sOtherFilePath As String) As Boolean
        Dim xCurNode As XmlElement = xXmlCurNode
        If xCurNode Is Nothing Then
            xCurNode = xXmlDoc.CreateElement("w", "p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
            xBodyEle.AppendChild(xCurNode)
        End If
        Try
            Dim xNewXmlDoc As XmlDocument = New XmlDocument()
            xNewXmlDoc.Load(sOtherFilePath)
            '需要拼接过去的word文档的root节点
            Dim xNewXmlRoot As XmlElement = xNewXmlDoc.GetElementsByTagName("pkg:package")(0)
            '获取需要拼接的word文档的对应关系的映射节点
            Dim xNewXmlRelationNode As XmlElement = GetEleByPkgName(xNewXmlRoot, "/word/_rels/document.xml.rels").GetElementsByTagName("Relationships")(0)
            '获取当前word文档的对应关系映射节点
            Dim xRelationsNode As XmlElement = GetEleByPkgName(xRootNode, "/word/_rels/document.xml.rels").GetElementsByTagName("Relationships")(0)
            Dim m_date As DateTime = DateTime.Now()
            '新文档的ID后缀
            Dim sNewID As String = m_date.Year().ToString() & m_date.Month().ToString() & m_date.Day().ToString() & m_date.Hour().ToString() & m_date.Minute().ToString() & m_date.Second().ToString()
            '需拼接文档的body节点
            Dim xNewXmlBody As XmlElement = xNewXmlRoot.GetElementsByTagName("w:body")(0)
            '需拼接文档的body节点内部xml转换为字符流
            Dim sNewBodyInnerStream As String = xNewXmlBody.InnerXml()
            Dim sRelationList As String = ""
            If Not xNewXmlRelationNode Is Nothing Then
                Dim xRelathions As XmlNodeList = xNewXmlRelationNode.ChildNodes()
                For i As Integer = 0 To xRelathions.Count() - 1
                    Dim sRelateId As String = CType(xRelathions(i), XmlElement).GetAttribute("Id")
                    Dim sRelateTarget As String = CType(xRelathions(i), XmlElement).GetAttribute("Target")
                    If Not (sRelateId = "rId1" Or sRelateId = "rId2" Or sRelateId = "rId3") Then
                        sRelationList = sRelationList & sRelateTarget & "," & sRelateId & ","
                        Dim sNTarget As String = ""
                        If sRelateTarget.IndexOf(".") >= 0 Then
                            Try
                                Convert.ToInt32(sRelateTarget.Substring(sRelateTarget.IndexOf(".") - 1, 1))
                                sNTarget = sRelateTarget.Substring(0, sRelateTarget.IndexOf(".")) & sNewID & "." & sRelateTarget.Substring(sRelateTarget.IndexOf(".") + 1, sRelateTarget.Length() - sRelateTarget.IndexOf(".") - 1)
                            Catch ex As Exception
                                sNTarget = sRelateTarget
                            End Try
                        Else
                            sNTarget = sRelateTarget
                        End If
                        sRelationList = sRelationList & sNTarget & "," & sRelateId & sNewID & ";"
                        CType(xRelathions(i), XmlElement).SetAttribute("Id", sRelateId & sNewID)
                        CType(xRelathions(i), XmlElement).SetAttribute("Target", sNTarget)
                        Dim xNewNode As XmlElement = xXmlDoc.ImportNode(xRelathions(i), True)
                        xRelationsNode.AppendChild(xNewNode)
                    End If
                Next
                Dim aRelationList As String() = sRelationList.Split(";")
                For i As Integer = 0 To aRelationList.Length - 1
                    If String.IsNullOrEmpty(aRelationList(i)) Then
                        Continue For
                    End If
                    Dim aOneRelation As String() = aRelationList(i).Split(",")
                    sNewBodyInnerStream = sNewBodyInnerStream.Replace("""" & aOneRelation(1) & """", """" & aOneRelation(3) & """")
                    Dim xNewXmlPkgNodes As XmlNodeList = xNewXmlRoot.ChildNodes()
                    For j As Integer = 0 To xNewXmlPkgNodes.Count() - 1
                        Dim sPKGName As String = CType(xNewXmlPkgNodes(i), XmlElement).GetAttribute("pkg:name")
                        If sPKGName.IndexOf(aOneRelation(0)) >= 0 Then
                            CType(xNewXmlPkgNodes(i), XmlElement).SetAttribute("pkg:name", sPKGName.Replace(aOneRelation(0), aOneRelation(2)))
                            Dim xNewNode As XmlElement = xXmlDoc.ImportNode(xNewXmlPkgNodes(i), True)
                            xRootNode.AppendChild(xNewNode)
                        End If
                    Next
                Next
            End If

            sNewBodyInnerStream = "<newbody>" & sNewBodyInnerStream & "</newbody>"
            xNewXmlDoc.LoadXml(sNewBodyInnerStream)
            xNewXmlRoot = xNewXmlDoc.ChildNodes()(0)
            Dim xNewBodyChildList As XmlNodeList = xNewXmlRoot.ChildNodes()
            For i As Integer = 0 To xNewBodyChildList.Count() - 1
                Dim xNewNode As XmlElement = xXmlDoc.ImportNode(xNewBodyChildList(i), True)
                xCurNode.ParentNode().InsertBefore(xNewNode, xCurNode)
            Next
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    

End Class
