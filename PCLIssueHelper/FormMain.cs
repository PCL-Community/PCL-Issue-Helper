using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;

namespace PCLIssueHelper
{
    public partial class FormMain : Form
    {
        public IssueSimilarityChecker checker;
        public FormMain()
        {
            InitializeComponent();
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "\\issues.json");
            List<Issues> issues = JsonSerializer.Deserialize<List<Issues>>(json) ?? new List<Issues>();

            checker = new(issues);
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void buttonGetSimilarity_Click(object sender, EventArgs e)
        {
            listViewTitle.Items.Clear();
            listViewBody.Items.Clear();
            var title = textBoxTitle.Text;
            var body = textBoxBody.Text;
            var similarity = checker.CheckSimilarity(textBoxTitle.Text, textBoxBody.Text);
            var similarityTitles = similarity.Item1;
            var similarityBodies = similarity.Item2;
            foreach (var _title in similarityTitles)
            {
                listViewTitle.Items.Add(new ListViewItem(new string[] { _title.Key, _title.Value.ToString(), checker._issues.Where(x => x.number.ToString() == _title.Key).Select(x => x.title).FirstOrDefault() ?? "" }));
            }
            foreach (var _body in similarityBodies)
            {
                listViewBody.Items.Add(new ListViewItem(new string[] { _body.Key, _body.Value.ToString(), checker._issues.Where(x => x.number.ToString() == _body.Key).Select(x => x.body).FirstOrDefault() ?? "" }));
            }
        }

        private void listViewTitle_DoubleClick(object sender, EventArgs e)
        {
            if (listViewTitle.SelectedItems.Count > 0)
            {
                var selectedItem = listViewTitle.SelectedItems[0];
                string id = selectedItem.SubItems[0].Text;
                string title = selectedItem.SubItems[2].Text;
                string body = checker._issues.Where(x => x.number.ToString() == id).Select(x => x.body).FirstOrDefault() ?? "";
                FormInfo formInfo = new(id, title, body);
                formInfo.ShowDialog();
            }
        }

        private void listViewBody_DoubleClick(object sender, EventArgs e)
        {
            if (listViewTitle.SelectedItems.Count > 0)
            {
                var selectedItem = listViewBody.SelectedItems[0];
                string id = selectedItem.SubItems[0].Text;
                string title = checker._issues.Where(x => x.number.ToString() == id).Select(x => x.title).FirstOrDefault() ?? "";
                string body = selectedItem.SubItems[2].Text;
                FormInfo formInfo = new(id, title, body);
                formInfo.ShowDialog();
            }
        }

        private void pCL2开源仓库RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"https://github.com/Hex-Dragon/PCL2") { UseShellExecute = true });
        }

        private void pCL2IssuesIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"https://github.com/Hex-Dragon/PCL2/issues") { UseShellExecute = true });
        }

        private void pCL2DiscussionsDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"https://github.com/Hex-Dragon/PCL2/discussions") { UseShellExecute = true });
        }

        private void 可以提交PR的IssuePToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"https://github.com/Hex-Dragon/PCL2/issues?q=is%3Aissue+is%3Aopen+label%3A%E5%A4%84%E7%90%86%E4%B8%AD") { UseShellExecute = true });
        }

        private void 需要社区帮忙的IssueHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"https://github.com/Hex-Dragon/PCL2/issues?q=label%3A%E6%96%B0%E6%8F%90%E4%BA%A4%2C%E9%9C%80%E8%A6%81%E7%A4%BE%E5%8C%BA%E5%A4%8D%E7%8E%B0%2C%E9%9C%80%E8%A6%81%E7%A4%BE%E5%8C%BA%E5%B8%AE%E5%8A%A9%2C%E7%A4%BE%E5%8C%BA%E5%A4%84%E7%90%86%E4%B8%AD+") { UseShellExecute = true });
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var taskDialogPage = new TaskDialogPage();
            taskDialogPage.EnableLinks = true;
            taskDialogPage.Caption = "关于";
            taskDialogPage.Heading = "关于 PCL Issue相似度对比器";
            taskDialogPage.Icon = TaskDialogIcon.Information;
            taskDialogPage.Text = """
                版本: 1.0.0
                作者: Hill233

                开放源代码许可:
                <a href="https://github.com/feature23/StringSimilarity.NET/blob/main/LICENSE">StringSimilarity.NET</a> Copyright © 2015 feature[23]
                <a href="https://github.com/xoofx/markdig/blob/master/license.txt">MarkDig</a> Copyright © 2018-2019, Alexandre Mutel All rights reserved.
                """;

            taskDialogPage.LinkClicked += (sender, e) =>
            {
                Process.Start(new ProcessStartInfo(e.LinkHref) {UseShellExecute = true });
            };

            TaskDialog.ShowDialog(taskDialogPage);
        }
    }
}