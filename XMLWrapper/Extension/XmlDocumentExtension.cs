using System.Xml;

namespace TestProject.XMLWrapper.Extension
{
    /// <summary>
    /// XmlDocument拡張クラス
    /// </summary>
    public class XmlDocumentExtension : XmlDocument
    {
        /// <summary>
        /// ルート要素を作成
        /// ※既存の子ノードはすべて削除します
        /// </summary>
        /// <param name="el">要素</param>
        /// <param name="version">バージョン</param>
        /// <param name="encoding">エンコーディング</param>
        /// <param name="standalone">外部依存</param>
        /// <returns>XmlElementWrapper</returns>
        public XmlElementWrapper CreateRootOfElementWrapper(string el, string version = "1.0", string encoding = "utf-8", string standalone = null)
        {
            this.RemoveAll();
            this.AppendChild(this.CreateXmlDeclaration(version, encoding, standalone));
            var createEl = this.CreateElement(el);
            return new XmlElementWrapper(this.AppendChild(createEl));
        }
    }

    /// <summary>
    /// XmlElementラッパークラス
    /// </summary>
    public class XmlElementWrapper
    {
        /// <summary>
        /// XmlElement
        /// </summary>
        private XmlElement _el;

        /// <summary>
        /// XmlDocument
        /// </summary>
        private XmlDocument _doc;

        /// <summary>
        /// タグ名
        /// </summary>
        public string Name => this._el.Name;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="node">XmlNode</param>
        public XmlElementWrapper(XmlNode node)
        {
            this._el = node as XmlElement;
            this._doc = this._el.OwnerDocument;
        }

        /// <summary>
        /// 子要素を追加
        /// </summary>
        /// <param name="el">要素</param>
        /// <returns>追加した子要素</returns>
        public XmlElementWrapper AppendChild(string el)
        {
            var addEl = this._el.AppendChild(this._doc.CreateElement(el));
            return new XmlElementWrapper(addEl);
        }

        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="value">値</param>
        public void SetValue(string value)
        {
            this._el.InnerText = value;
        }

        /// <summary>
        /// 値を取得
        /// </summary>
        /// <returns>値</returns>
        public string GetValue()
        {
            return this._el.InnerText;
        }

        /// <summary>
        /// 属性を設定
        /// </summary>
        /// <param name="attrName">属性名</param>
        /// <param name="value">属性値</param>
        public void SetAttribute(string attrName, string attrValue)
        {
            this._el.SetAttribute(attrName, attrValue);
        }

        /// <summary>
        /// 属性を取得
        /// </summary>
        /// <param name="attrName">属性名</param>
        /// <returns>属性値</returns>
        public string GetAttribute(string attrName)
        {
            return this._el.GetAttribute(attrName);
        }

        /// <summary>
        /// 要素検索
        /// </summary>
        /// <param name="xPath">XPath</param>
        /// <returns>XmlElementWrapper</returns>
        public XmlElementWrapper SelectSingleNode(string xPath)
        {
            var node = this._el.SelectSingleNode(xPath);
            return node == null ? null : new XmlElementWrapper(node);
        }
    }
}
