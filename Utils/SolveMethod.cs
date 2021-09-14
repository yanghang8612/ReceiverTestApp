using ReceiverTestApp.Domain;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace ReceiverTestApp.Utils
{
    public struct hcltime_t
    {        /* 以1970年1月1日0时为零时至今的时间 */
        public long time;        /* 秒的整数部分 */
        public double sec;         /* 秒的小数部分 */
    }

    public struct ecef_position_s
    {
        public hcltime_t time; //时间（s）
        public double hdop;  //HDOP值
        public int ns;    //定位卫星数量
        public int stat;  //定位状态 0:定位不可用 1:单点定位 2:差分定位 4:RTK固定解 5:RTK浮点解
        public double x;     //地心地固坐标系X坐标（m）
        public double y;     //地心地固坐标系Y坐标（m）
        public double z;     //地心地固坐标系Z坐标（m）
        public double lat;   //纬度（deg）
        public double lon;   //经度（deg）
        public double height;//高度（deg）
    }

    public struct enu_position_s
    {
        public hcltime_t time; //时间（s）
        public double e;  //当地坐标系东向（east）坐标（m）
        public double n;  //当地坐标系北向（north）坐标（m）
        public double u;  //当地坐标系高程（up）坐标（m）
    }

    public struct track_satellite_number_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_rinex_file;  //接收机RINEX原始观测量文件
        public int signal_type;  //信号类型
    }

    public struct track_satellite_number_output_para_s
    {
        public int signal_type;  //信号类型
        public int track_satellite_number;   //跟踪卫星数量
    }

    public struct signal_type_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;  //信号类型
    }

    public struct signal_type_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数	
    }

    public struct pseudorange_accuracy_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_rinex_file;  //接收机RINEX原始观测量文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_rinex_file;   //模拟源RINEX原始观测量文件
        public int signal_type;  //信号类型
    }

    public struct pseudorange_accuracy_output_para_s
    {
        public double accuracy;  //伪距测量精度(m)
        public IntPtr time;  //时间数组
        public IntPtr data;     //伪距测量误差数组(m)
        public double ndata;     //伪距测量误差元素个数	
    }

    public struct carrier_phase_accuracy_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_rinex_file;  //接收机RINEX原始观测量文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_rinex_file;   //模拟源RINEX原始观测量文件
        public int signal_type;  //信号类型
    }

    public struct carrier_phase_accuracy_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracy;  //载波相位测量精度(m)
        public IntPtr time;  //时间数组
        public IntPtr data;     //载波相位测量误差数组(m)
        public double ndata;     //载波相位测量误差元素个数	
    }

    public struct acquisition_sensitivity_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public double power;     //至射频输入信号口功率
        public int signal_type;
    }

    public struct acquisition_sensitivity_output_para_s
    {
        public int signal_type;    //信号类型
        public double accuracy3D;  //三维定位误差(m)
        public double power;      //至射频输入信号口功率
    }

    public struct reacquisition_sensitivity_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public double power;     //至射频输入信号口功率
        public int signal_type;
    }

    public struct reacquisition_sensitivity_output_para_s
    {
        public int signal_type;    //信号类型
        public double accuracy3D;  //三维定位误差(m)
        public double power;      //至射频输入信号口功率
    }

    public struct tracking_sensitivity_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public double power;     //至射频输入信号口功率
        public int signal_type;
    }

    public struct tracking_sensitivity_output_para_s
    {
        public int signal_type;    //信号类型
        public double accuracy3D;  //三维定位误差(m)
        public double power;      //至射频输入信号口功率
    }

    public struct doppler_accuracy_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_rinex_file;  //接收机RINEX原始观测量文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_rinex_file;   //模拟源RINEX原始观测量文件
        public int signal_type;  //信号类型
    }

    public struct doppler_accuracy_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracy;  //多普勒测量精度(Hz)
        public IntPtr time;  //时间数组
        public IntPtr data;     //多普勒测量误差数组(Hz)
        public double ndata;     //多普勒测量误差元素个数	
    }

    public struct cold_start_time_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_tag_file;  //接收机NMEA时间记录文件
        public double time_begin;             //开始记录时间
        public int signal_type;  //信号类型
    }

    public struct cold_start_time_output_para_s
    {
        public int signal_type;  //信号类型
        public double time;  //冷启动时间(s)
    }

    public struct warm_start_time_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_tag_file;  //接收机NMEA时间记录文件
        public double time_begin;             //开始记录时间
        public int signal_type;  //信号类型
    }

    public struct warm_start_time_output_para_s
    {
        public int signal_type;  //信号类型
        public double time;  //温启动时间(s)
    }

    public struct hot_start_time_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_tag_file;  //接收机NMEA时间记录文件
        public double time_begin;             //开始记录时间
        public int signal_type;  //信号类型
    }

    public struct hot_start_time_output_para_s
    {
        public int signal_type;  //信号类型
        public double time;  //热启动时间(s)
    }

    public struct signal_reacquisition_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_tag_file;  //接收机NMEA时间记录文件
        public double time_begin;             //开始记录时间
        public int signal_type;  //信号类型
    }
    
    public struct signal_reacquisition_output_para_s
    {
        public int signal_type;  //信号类型
        public double time;  //失锁重捕获时间(s)
    }

    public struct rtk_initialize_time_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_tag_file;  //接收机NMEA时间记录文件
        public double time_begin;             //开始记录时间
        public int signal_type;  //信号类型
    }

    public struct rtk_initialize_time_output_para_s
    {
        public int signal_type;  //信号类型
        public double time;  //rtk初始化时间(s)
    }

    public struct internal_noise_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_rinex_file;  //接收机RINEX原始观测量文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_rinex_file;   //模拟源RINEX原始观测量文件
        public int signal_type;  //信号类型
    }

    public struct internal_noise_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracy;  //内部噪声水平(m)
        public IntPtr time;  //时间数组
        public IntPtr data;     //内部噪声误差数组(m)
        public double ndata;     //内部噪声误差元素个数
    }

    public struct static_single_point_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;
    }

    public struct static_single_point_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数
    }

    public struct dynamic_single_point_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;
    }

    public struct dynamic_single_point_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数
    }

    public struct static_baseline_accuracy_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;
    }

    public struct static_baseline_accuracy_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数
    }

    public struct rtk_accuracy_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;
    }

    public struct rtk_accuracy_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数
    }

    public struct speed_accuracy_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;
    }

    public struct speed_accuracy_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracy;  //测速精度(m)
        public IntPtr time;   //时间数组
        public IntPtr data;     //测速误差数组
        public double ndata;     //定位误差数组元素个数
    }

    public struct interoperate_performance_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;
    }

    public struct interoperate_performance_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数
    }

    public struct dynamic_performance_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;  //信号类型
    }

    public struct dynamic_performance_output_para_s
    {
        public int signal_type;   //信号类型
        public double accuracyE;  //东向定位精度(m)
        public double accuracyN;  //北向定位精度(m)
        public double accuracyU;  //垂直定位精度(m)
        public double accuracy2D; //水平定位精度(m)
        public double accuracy3D; //三维定位精度(m)
        public IntPtr time;   //时间数组
        public IntPtr dataE;     //东向定位误差数组
        public IntPtr dataN;     //北向定位误差数组
        public IntPtr dataU;     //垂直定位误差数组
        public IntPtr data2D;    //水平定位误差数组
        public IntPtr data3D;    //三维定位误差数组
        public IntPtr dataNS;    //定位卫星数量数组
        public double ndata;     //定位误差数组元素个数	
    }

    public struct data_updating_rate_input_para_s
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string recv_nmea_file;      //接收机NMEA文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string sim_nmea_file;       //模拟源NMEA文件
        public int signal_type;  //信号类型
    }

    public struct data_updating_rate_output_para_s
    {
        public double accuracy3D; //三维定位精度(m)
        public double rate;  //数据更新率(Hz)
    }

    class SolveMethod
    {
        //跟踪卫星通道数接口
        [DllImport(@"hcllib.dll")]
        public static extern track_satellite_number_output_para_s hcl_track_satellite_number(track_satellite_number_input_para_s[] input_para);

        public static double[] TrackSatelliteNumberByRINEX(string filePath, int signalType)
        {
            track_satellite_number_input_para_s[] param = new track_satellite_number_input_para_s[1];
            param[0].recv_rinex_file = filePath;
            param[0].signal_type = signalType;
            track_satellite_number_output_para_s res = hcl_track_satellite_number(param);
            double[] ret = new double[1];
            ret[0] = res.track_satellite_number;
            return ret;
        }

        //捕获灵敏度接口
        [DllImport(@"hcllib.dll")]
        public static extern acquisition_sensitivity_output_para_s hcl_acquisition_sensitivity(acquisition_sensitivity_input_para_s[] input_para);

        public static double[] AcquisitionSensitivityByNMEA(string recv, string sim, double power, int signalType)
        {
            acquisition_sensitivity_input_para_s[] param = new acquisition_sensitivity_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].power = power;
            param[0].signal_type = signalType;
            acquisition_sensitivity_output_para_s res = hcl_acquisition_sensitivity(param);
            double[] ret = new double[1];
            ret[0] = res.accuracy3D;
            return ret;
        }

        //重捕获灵敏度接口
        [DllImport(@"hcllib.dll")]
        public static extern reacquisition_sensitivity_output_para_s hcl_reacquisition_sensitivity(reacquisition_sensitivity_input_para_s[] input_para);

        public static double[] ReacquisitionSensitivityByNMEA(string recv, string sim, double power, int signalType)
        {
            reacquisition_sensitivity_input_para_s[] param = new reacquisition_sensitivity_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].power = power;
            param[0].signal_type = signalType;
            reacquisition_sensitivity_output_para_s res = hcl_reacquisition_sensitivity(param);
            double[] ret = new double[1];
            ret[0] = res.accuracy3D;
            return ret;
        }

        //跟踪灵敏度接口
        [DllImport(@"hcllib.dll")]
        public static extern tracking_sensitivity_output_para_s hcl_tracking_sensitivity(tracking_sensitivity_input_para_s[] input_para);

        public static double[] TrackingSensitivityByNMEA(string recv, string sim, double power, int signalType)
        {
            tracking_sensitivity_input_para_s[] param = new tracking_sensitivity_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].power = power;
            param[0].signal_type = signalType;
            tracking_sensitivity_output_para_s res = hcl_tracking_sensitivity(param);
            double[] ret = new double[1];
            ret[0] = res.accuracy3D;
            return ret;
        }

        //冷启动时间接口
        [DllImport(@"hcllib.dll")]
        public static extern cold_start_time_output_para_s hcl_cold_start_time(cold_start_time_input_para_s[] input_para);

        public static double[] ColdStartTimeByNMEA(string recv, string sim, string time, double beginTime, int signalType)
        {
            cold_start_time_input_para_s[] param = new cold_start_time_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].recv_nmea_tag_file = time;
            param[0].time_begin = beginTime;
            param[0].signal_type = signalType;
            cold_start_time_output_para_s res = hcl_cold_start_time(param);
            double[] ret = new double[1];
            ret[0] = res.time;
            return ret;
        }

        //温启动时间接口
        [DllImport(@"hcllib.dll")]
        public static extern warm_start_time_output_para_s hcl_warm_start_time(warm_start_time_input_para_s[] input_para);

        public static double[] WarmStartTimeByNMEA(string recv, string sim, string time, double beginTime, int signalType)
        {
            warm_start_time_input_para_s[] param = new warm_start_time_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].recv_nmea_tag_file = time;
            param[0].time_begin = beginTime;
            param[0].signal_type = signalType;
            warm_start_time_output_para_s res = hcl_warm_start_time(param);
            double[] ret = new double[1];
            ret[0] = res.time;
            return ret;
        }

        //热启动时间接口
        [DllImport(@"hcllib.dll")]
        public static extern hot_start_time_output_para_s hcl_hot_start_time(hot_start_time_input_para_s[] input_para);

        public static double[] HotStartTimeByNMEA(string recv, string sim, string time, double beginTime, int signalType)
        {
            hot_start_time_input_para_s[] param = new hot_start_time_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].recv_nmea_tag_file = time;
            param[0].time_begin = beginTime;
            param[0].signal_type = signalType;
            hot_start_time_output_para_s res = hcl_hot_start_time(param);
            double[] ret = new double[1];
            ret[0] = res.time;
            return ret;
        }

        //信号失锁重捕获时间接口
        [DllImport(@"hcllib.dll")]
        public static extern signal_reacquisition_output_para_s hcl_signal_reacquisition_time(signal_reacquisition_input_para_s[] input_para);

        public static double[] SignalReacquisitionTimeByNMEA(string recv, string sim, string time, double beginTime, int signalType)
        {
            signal_reacquisition_input_para_s[] param = new signal_reacquisition_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].recv_nmea_tag_file = time;
            param[0].time_begin = beginTime;
            param[0].signal_type = signalType;
            signal_reacquisition_output_para_s res = hcl_signal_reacquisition_time(param);
            double[] ret = new double[1];
            ret[0] = res.time;
            return ret;
        }

        //RTK初始化时间接口
        [DllImport(@"hcllib.dll")]
        public static extern rtk_initialize_time_output_para_s hcl_rtk_initialize_time(rtk_initialize_time_input_para_s[] input_para);

        public static double[] RtkInitializeTimeByNMEA(string recv, string sim, string time, double beginTime, int signalType)
        {
            rtk_initialize_time_input_para_s[] param = new rtk_initialize_time_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].recv_nmea_tag_file = time;
            param[0].time_begin = beginTime;
            param[0].signal_type = signalType;
            rtk_initialize_time_output_para_s res = hcl_rtk_initialize_time(param);
            double[] ret = new double[1];
            ret[0] = res.time;
            return ret;
        }

        //内部噪声水平接口
        [DllImport(@"hcllib.dll")]
        public static extern internal_noise_output_para_s hcl_internal_noise_level(internal_noise_input_para_s[] input_para);

        public static double[] InternalNoiseLevelByRINEX(string recv, string sim, Freq signal, string path)
        {
            internal_noise_input_para_s[] param = new internal_noise_input_para_s[1];
            param[0].recv_rinex_file = recv;
            param[0].sim_rinex_file = sim;
            param[0].signal_type = signal.Value;
            internal_noise_output_para_s res = hcl_internal_noise_level(param);
            RecordToFile(res.ndata, res.time, res.data, path + "\\" + signal.Name + ".dat");
            double[] ret = new double[1];
            ret[0] = res.accuracy;
            return ret;
        }

        //伪距测量精度接口
        [DllImport(@"hcllib.dll")]
        public static extern pseudorange_accuracy_output_para_s hcl_pseudorange_accuracy(pseudorange_accuracy_input_para_s[] input_para);

        public static double[] PseudorangeAccuracyByRINEX(string recv, string sim, Freq signal, string path)
        {
            pseudorange_accuracy_input_para_s[] param = new pseudorange_accuracy_input_para_s[1];
            param[0].recv_rinex_file = recv;
            param[0].sim_rinex_file = sim;
            param[0].signal_type = signal.Value;
            pseudorange_accuracy_output_para_s res = hcl_pseudorange_accuracy(param);
            RecordToFile(res.ndata, res.time, res.data, path + "\\" + signal.Name + ".dat");
            double[] ret = new double[1];
            ret[0] = res.accuracy;
            return ret;
        }

        //载波相位测量精度接口
        [DllImport(@"hcllib.dll")]
        public static extern carrier_phase_accuracy_output_para_s hcl_carrier_phase_accuracy(carrier_phase_accuracy_input_para_s[] input_para);

        public static double[] CarrierPhaseAccuracyByRINEX(string recv, string sim, Freq signal, string path)
        {
            carrier_phase_accuracy_input_para_s[] param = new carrier_phase_accuracy_input_para_s[1];
            param[0].recv_rinex_file = recv;
            param[0].sim_rinex_file = sim;
            param[0].signal_type = signal.Value;
            carrier_phase_accuracy_output_para_s res = hcl_carrier_phase_accuracy(param);
            RecordToFile(res.ndata, res.time, res.data, path + "\\" + signal.Name + ".dat");
            double[] ret = new double[1];
            ret[0] = res.accuracy;
            return ret;
        }

        //多普勒测量精度接口
        [DllImport(@"hcllib.dll")]
        public static extern doppler_accuracy_output_para_s hcl_doppler_accuracy(doppler_accuracy_input_para_s[] input_para);

        public static double[] DopplerAccuracyByRINEX(string recv, string sim, Freq signal, string path)
        {
            doppler_accuracy_input_para_s[] param = new doppler_accuracy_input_para_s[1];
            param[0].recv_rinex_file = recv;
            param[0].sim_rinex_file = sim;
            param[0].signal_type = signal.Value;
            doppler_accuracy_output_para_s res = hcl_doppler_accuracy(param);
            RecordToFile(res.ndata, res.time, res.data, path + "\\" + signal.Name + ".dat");
            double[] ret = new double[1];
            ret[0] = res.accuracy;
            return ret;
        }

        //单点静态定位精度测试接口
        [DllImport(@"hcllib.dll")]
        public static extern static_single_point_output_para_s hcl_static_single_point(static_single_point_input_para_s[] input_para);

        public static double[] StaticSinglePointByNMEA(string recv, string sim, Freq signal, string path)
        {
            static_single_point_input_para_s[] param = new static_single_point_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            static_single_point_output_para_s res = hcl_static_single_point(param);
            RecordToFile(res.ndata, res.time, res.dataE, path + "\\" + signal.Name + ".E");
            RecordToFile(res.ndata, res.time, res.dataN, path + "\\" + signal.Name + ".N");
            RecordToFile(res.ndata, res.time, res.dataU, path + "\\" + signal.Name + ".U");
            RecordToFile(res.ndata, res.time, res.data2D, path + "\\" + signal.Name + ".2D");
            RecordToFile(res.ndata, res.time, res.data3D, path + "\\" + signal.Name + ".3D");
            RecordToFile(res.ndata, res.time, res.dataNS, path + "\\" + signal.Name + ".NS");
            double[] ret = new double[5];
            ret[0] = res.accuracyE;
            ret[1] = res.accuracyN;
            ret[2] = res.accuracyU;
            ret[3] = res.accuracy2D;
            ret[4] = res.accuracy3D;
            return ret;
        }

        //单点动态定位精度测试接口
        [DllImport(@"hcllib.dll")]
        public static extern dynamic_single_point_output_para_s hcl_dynamic_single_point(dynamic_single_point_input_para_s[] input_para);

        public static double[] DynamicSinglePointByNMEA(string recv, string sim, Freq signal, string path)
        {
            dynamic_single_point_input_para_s[] param = new dynamic_single_point_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            dynamic_single_point_output_para_s res = hcl_dynamic_single_point(param);
            RecordToFile(res.ndata, res.time, res.dataE, path + "\\" + signal.Name + ".E");
            RecordToFile(res.ndata, res.time, res.dataN, path + "\\" + signal.Name + ".N");
            RecordToFile(res.ndata, res.time, res.dataU, path + "\\" + signal.Name + ".U");
            RecordToFile(res.ndata, res.time, res.data2D, path + "\\" + signal.Name + ".2D");
            RecordToFile(res.ndata, res.time, res.data3D, path + "\\" + signal.Name + ".3D");
            RecordToFile(res.ndata, res.time, res.dataNS, path + "\\" + signal.Name + ".NS");
            double[] ret = new double[5];
            ret[0] = res.accuracyE;
            ret[1] = res.accuracyN;
            ret[2] = res.accuracyU;
            ret[3] = res.accuracy2D;
            ret[4] = res.accuracy3D;
            return ret;
        }

        //静态基线测量精度测试接口
        [DllImport(@"hcllib.dll")]
        public static extern static_baseline_accuracy_output_para_s hcl_static_baseline_accuracy(static_baseline_accuracy_input_para_s[] input_para);

        public static double[] StaticBaselineAccuracyByNMEA(string recv, string sim, Freq signal, string path)
        {
            static_baseline_accuracy_input_para_s[] param = new static_baseline_accuracy_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            static_baseline_accuracy_output_para_s res = hcl_static_baseline_accuracy(param);
            RecordToFile(res.ndata, res.time, res.dataE, path + "\\" + signal.Name + ".E");
            RecordToFile(res.ndata, res.time, res.dataN, path + "\\" + signal.Name + ".N");
            RecordToFile(res.ndata, res.time, res.dataU, path + "\\" + signal.Name + ".U");
            RecordToFile(res.ndata, res.time, res.data2D, path + "\\" + signal.Name + ".2D");
            RecordToFile(res.ndata, res.time, res.data3D, path + "\\" + signal.Name + ".3D");
            RecordToFile(res.ndata, res.time, res.dataNS, path + "\\" + signal.Name + ".NS");
            double[] ret = new double[5];
            ret[0] = res.accuracyE;
            ret[1] = res.accuracyN;
            ret[2] = res.accuracyU;
            ret[3] = res.accuracy2D;
            ret[4] = res.accuracy3D;
            return ret;
        }

        //RTK测量精度测试接口
        [DllImport(@"hcllib.dll")]
        public static extern rtk_accuracy_output_para_s hcl_rtk_accuracy(rtk_accuracy_input_para_s[] input_para);

        public static double[] RtkAccuracyByNMEA(string recv, string sim, Freq signal, string path)
        {
            rtk_accuracy_input_para_s[] param = new rtk_accuracy_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            rtk_accuracy_output_para_s res = hcl_rtk_accuracy(param);
            RecordToFile(res.ndata, res.time, res.dataE, path + "\\" + signal.Name + ".E");
            RecordToFile(res.ndata, res.time, res.dataN, path + "\\" + signal.Name + ".N");
            RecordToFile(res.ndata, res.time, res.dataU, path + "\\" + signal.Name + ".U");
            RecordToFile(res.ndata, res.time, res.data2D, path + "\\" + signal.Name + ".2D");
            RecordToFile(res.ndata, res.time, res.data3D, path + "\\" + signal.Name + ".3D");
            RecordToFile(res.ndata, res.time, res.dataNS, path + "\\" + signal.Name + ".NS");
            double[] ret = new double[5];
            ret[0] = res.accuracyE;
            ret[1] = res.accuracyN;
            ret[2] = res.accuracyU;
            ret[3] = res.accuracy2D;
            ret[4] = res.accuracy3D;
            return ret;
        }

        //测速精度测试接口
        [DllImport(@"hcllib.dll")]
        public static extern speed_accuracy_output_para_s hcl_speed_accuracy(speed_accuracy_input_para_s[] input_para);

        public static double SpeedAccuracyByNMEA(string recv, string sim, Freq signal, string path, int idx)
        {
            speed_accuracy_input_para_s[] param = new speed_accuracy_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            speed_accuracy_output_para_s res = hcl_speed_accuracy(param);
            RecordToFile(res.ndata, res.time, res.data, path + "\\" + signal.Name + "." + idx);
            return res.accuracy;
        }

        //互操作性能测试接口
        [DllImport(@"hcllib.dll")]
        public static extern interoperate_performance_output_para_s hcl_interoperate_performance(interoperate_performance_input_para_s[] input_para);

        public static double[] InteroperatePerformanceByNMEA(string recv, string sim, Freq signal, string path)
        {
            interoperate_performance_input_para_s[] param = new interoperate_performance_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            interoperate_performance_output_para_s res = hcl_interoperate_performance(param);
            RecordToFile(res.ndata, res.time, res.dataE, path + "\\" + signal.Name + ".E");
            RecordToFile(res.ndata, res.time, res.dataN, path + "\\" + signal.Name + ".N");
            RecordToFile(res.ndata, res.time, res.dataU, path + "\\" + signal.Name + ".U");
            RecordToFile(res.ndata, res.time, res.data2D, path + "\\" + signal.Name + ".2D");
            RecordToFile(res.ndata, res.time, res.data3D, path + "\\" + signal.Name + ".3D");
            RecordToFile(res.ndata, res.time, res.dataNS, path + "\\" + signal.Name + ".NS");
            double[] ret = new double[5];
            ret[0] = res.accuracyE;
            ret[1] = res.accuracyN;
            ret[2] = res.accuracyU;
            ret[3] = res.accuracy2D;
            ret[4] = res.accuracy3D;
            return ret;
        }

        //动态性能测试接口
        [DllImport(@"hcllib.dll")]
        public static extern dynamic_performance_output_para_s hcl_dynamic_performance(dynamic_performance_input_para_s[] input_para);

        public static double[] DynamicPerformanceByNMEA(string recv, string sim, Freq signal, string path)
        {
            dynamic_performance_input_para_s[] param = new dynamic_performance_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signal.Value;
            dynamic_performance_output_para_s res = hcl_dynamic_performance(param);
            RecordToFile(res.ndata, res.time, res.dataE, path + "\\" + signal.Name + ".E");
            RecordToFile(res.ndata, res.time, res.dataN, path + "\\" + signal.Name + ".N");
            RecordToFile(res.ndata, res.time, res.dataU, path + "\\" + signal.Name + ".U");
            RecordToFile(res.ndata, res.time, res.data2D, path + "\\" + signal.Name + ".2D");
            RecordToFile(res.ndata, res.time, res.data3D, path + "\\" + signal.Name + ".3D");
            RecordToFile(res.ndata, res.time, res.dataNS, path + "\\" + signal.Name + ".NS");
            double[] ret = new double[5];
            ret[0] = res.accuracyE;
            ret[1] = res.accuracyN;
            ret[2] = res.accuracyU;
            ret[3] = res.accuracy2D;
            ret[4] = res.accuracy3D;
            return ret;
        }

        //数据更新率接口
        [DllImport(@"hcllib.dll")]
        public static extern data_updating_rate_output_para_s hcl_data_updating_rate(data_updating_rate_input_para_s[] input_para);

        public static double[] DataUpdatingRateByNMEA(string recv, string sim, int signalType)
        {
            data_updating_rate_input_para_s[] param = new data_updating_rate_input_para_s[1];
            param[0].recv_nmea_file = recv;
            param[0].sim_nmea_file = sim;
            param[0].signal_type = signalType;
            data_updating_rate_output_para_s res = hcl_data_updating_rate(param);
            double[] ret = new double[2];
            ret[0] = res.rate;
            ret[1] = res.accuracy3D;
            return ret;
        }

        //接收信号类型接口
        [DllImport(@"hcllib.dll")]
        public static extern signal_type_output_para_s hcl_signal_type(signal_type_input_para_s[] input_para);

        [DllImport(@"hcllib.dll")]
        public static extern int hcl_input(string input_str, [In, Out] ecef_position_s[] ecef_pos);

        public static ecef_position_s SolveGGA(string input_str)
        {
            ecef_position_s[] p = new ecef_position_s[1];
            hcl_input(input_str, p);
            return p[0];
        }

        [DllImport(@"hcllib.dll")]
        public static extern enu_position_s hcl_ecef2enu(ecef_position_s recv_pos, ecef_position_s sim_pos);

        public static enu_position_s Compare(string recv_str, string sim_str)
        {
            return hcl_ecef2enu(SolveGGA(recv_str), SolveGGA(sim_str));
        }

        [DllImport(@"hcllib.dll")]
        public static extern void hcltime2epoch(hcltime_t time, ref double year, ref double month, ref double day, ref double hour, ref double min, ref double sec);

        public static void RecordToFile(double dn, IntPtr time, IntPtr data, string file)
        {
            int n = (int)dn;
            hcltime_t[] t = new hcltime_t[n];
            for (int i = 0; i < n; i++, time = new IntPtr(time.ToInt32() + Marshal.SizeOf(typeof(hcltime_t))))
            {
                t[i] = Marshal.PtrToStructure<hcltime_t>(time);
            }
            double[] d = new double[n];
            for (int i = 0; i < n; i++, data = new IntPtr(data.ToInt32() + Marshal.SizeOf(typeof(double))))
            {
                d[i] = Marshal.PtrToStructure<double>(data);
            }
            using (StreamWriter writer = new StreamWriter(file))
            {
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                for (int i = 0; i < n; i++)
                {
                    writer.WriteLine(string.Format("{0}\t{1}", 
                        new DateTime(1970, 1, 1).AddSeconds(t[i].time).ToString("yy/MM/dd\tHH:mm:ss.ff", dtFormat), d[i]));
                }
            }
        }
    }
}