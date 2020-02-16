namespace Reports
{
    public abstract class JtdsConnection
    {
        public readonly static string TASK_FILL = "fill";
        public readonly static string TASK_PRINT = "print";
        public readonly static string TASK_PDF = "pdf";
        public readonly static string TASK_HTML = "html";
        public readonly static string TASK_RTF = "rtf";
        public readonly static string TASK_XLSX = "xlsx";
        public readonly static string TASK_JXL = "jxl";
        public readonly static string TASK_CSV = "csv";
        public readonly static string TASK_DOCX = "docx";
        public readonly static string TASK_PPTX = "pptx";
        public readonly static string TASK_TEXT = "txt";
        public readonly static string TASK_XLS = "xls";
        //public readonly static string TASK_XML = "xml";
        //public readonly static string TASK_XML_EMBED = "xmlEmbed";
        //public readonly static string TASK_ODT = "odt";
        //public readonly static string TASK_RUN = "run";
        //public readonly static string TASK_COMPILE = "compile";
        public abstract void ExpReort(string taskName);
    }
}
