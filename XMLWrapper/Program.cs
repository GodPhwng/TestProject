﻿using System;
using System.IO;
using TestProject.XMLWrapper.Extension;

namespace TestProject.XMLWrapper
{
    /// <summary>
    /// メインクラス
    /// </summary>
    public class Program
    {
        /// <summary>
        /// メインエントリ
        /// </summary>
        /// <param name="args">実行時引数</param>
        public static void Main(string[] args)
        {
            try
            {
                // XmlDocument拡張クラス
                var doc = new XmlDocumentExtension();

                // root要素作成
                var root = doc.CreateRootOfElementWrapper("root");

                // rootの直下にtestAを作成
                var testA = root.AppendChild("testA");

                // testAの下にtestBを作成
                var testB = testA.AppendChild("testB");

                // testBの下に同じタグを追加
                var testTag1 = testB.AppendChild("testTag");
                var testTag2 = testB.AppendChild("testTag");

                // 値設定
                var value1 = "valueTag1";
                var value2 = "valueTag2";
                testTag1.SetValue(value1);
                testTag2.SetValue(value2);

                // 属性設定
                testTag1.SetAttribute("testAttr", "testAttrValue");

                // XPath
                var findEl = root.SelectSingleNode($"//testB/testTag[text()='{value1}']");

                Console.WriteLine("SelectSingleNode結果 XMLタグ名：" + findEl?.Name);
                Console.WriteLine("属性testAttr：" + findEl?.GetAttribute("testAttr"));

                // xml保存
                doc.Save(Path.Combine(Environment.CurrentDirectory, "test.xml"));
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);                
            }
            finally
            {
                Console.Read();
            }
        }
    }
}
