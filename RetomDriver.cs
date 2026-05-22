
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace ModeRetomer
{
    public class RetomDriver
    {
        public RTDI.DualServer m_retom = null;//new RTDI.DualServer();
        public string m_stError = "null";
        public string m_stFunction = "null";
        public int m_nResultReturn = 0;
        public int m_nIsOpen;
        public string m_stResult = "";
        public List<bool> Contacts = Enumerable.Repeat(false, 16).ToList();
        public List<bool> ContactsZero = Enumerable.Repeat(false, 16).ToList();
        public List<double> AnalGr1 = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public List<double> AnalGr2 = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public List<double> AnalGr3 = new List<double> { 0, 0, 0 };
        public List<double> AnalGr4 = new List<double> { 0, 0, 0 };
        public bool CreateRetom()
        {
            m_nIsOpen = 0; // Сбрасываем флаг открытия
            m_stError = string.Empty; // Очищаем предыдущие ошибки

            try
            {
                m_retom = new RTDI.DualServer();

                // Добавляем проверку успешности создания
                if (m_retom != null)
                {
                    m_nIsOpen = 1; // Устанавливаем флаг успешного создания
                    //m_retom.SetMaxUI(100, 10); неработает
                    return true;
                }

                m_stError = "Failed to create DualServer instance";
                return false;
            }
            catch (Exception ex) // Конкретное исключение лучше, чем общий catch
            {
                m_stError = $"Error_CreateRetom: {ex.Message}";
                // Дополнительное логирование для диагностики
                Debug.WriteLine($"CreateRetom failed: {ex.ToString()}");
                return false;
            }
        }

        public void RunRetom()
        {
            m_stResult = "";
            try
            {
                if (m_stFunction == "Open")
                {

                    Console.WriteLine(m_nResultReturn.ToString());
                    if (m_retom == null)
                    {
                        string message = "Объект m_retom не инициализирован (равен null).\n\n" +
                                       "Проверьте инициализацию объекта в конструкторе или методе инициализации.";
                        string caption = "Ошибка инициализации";

                        MessageBox.Show(message, caption,
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                        return;
                    }

                    m_nIsOpen = 0;
                    m_nResultReturn = m_retom.Close();
                    m_nResultReturn = m_retom.SetWorkParam("RetomType_66");
                    m_nIsOpen = m_retom.Open("IP:192.168.11.146", 0);
                    //m_nIsOpen = m_retom.Open("IP:169.254.213.240", 0);
                    //m_nResultReturn = m_retom.SetWorkParam("RetomType_51");
                    //m_nIsOpen = m_retom.Open("USB", 0);
                    m_nResultReturn = m_nIsOpen;
                    //var testtest = m_retom.ServerInfo;
                    Console.WriteLine(m_nResultReturn.ToString());
                    if (m_retom == null)
                    {
                        string message = "Объект m_retom не инициализирован (равен null).\n\n" +
                                       "Проверьте инициализацию объекта в конструкторе или методе инициализации.";
                        string caption = "Ошибка инициализации";

                        MessageBox.Show(message, caption,
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                        return;
                    }

                }
                else
                    if (m_nIsOpen > 0) 
                {
                    switch (m_stFunction)
                    {
                        //case "SetOutContact":
                            //short nNumCont = 0;
                            //Contacts.ForEach(value => 
                            //{
                                //m_nResultReturn = m_retom.SetOutContact(nNumCont, value);
                                //nNumCont++;
                            //}
                            //);

                        case "SetOutContact":
                            short nNumCont = 0;
                            foreach (var value in Contacts.Take(16)) // первые 8 элементов (индексы 0–7)
                            {
                                m_nResultReturn = m_retom.SetOutContact(nNumCont, value);
                                nNumCont++;
                            }
                            break;

                        case "Close":
                            m_nResultReturn = m_retom.Close();
                            if (m_nResultReturn > 0)
                                m_nIsOpen = 0;
                            break;
                        case "Enable":
                            m_nResultReturn = m_retom.Enable();
                            break;
                        case "Disable":
                            m_nResultReturn = m_retom.Disable();
                            break;
                        case "Out61":

                            RTDI.CoAnalogOutputs channels = m_retom.NewAnalogChannels();
                            channels.dFrequency = 50;
                            channels.SetSinSignal(nChannel: 0, dAmpl: AnalGr1[6], dPhase: AnalGr1[7]);
                            channels.SetSinSignal(nChannel: 1, dAmpl: AnalGr1[8], dPhase: AnalGr1[9]);
                            channels.SetSinSignal(nChannel: 2, dAmpl: AnalGr1[10], dPhase: AnalGr1[11]);
                            channels.SetSinSignal(nChannel: 3, dAmpl: AnalGr1[0], dPhase: AnalGr1[1]);
                            channels.SetSinSignal(nChannel: 4, dAmpl: AnalGr1[2], dPhase: AnalGr1[3]);
                            channels.SetSinSignal(nChannel: 5, dAmpl: AnalGr1[4], dPhase: AnalGr1[5]);

                            //channels.AddHarmonica(nChannel: 3, dAmpl: AnalGr1[0], dPhase: AnalGr1[1], dFreq: 50, dExp: 0);
                            //channels.AddHarmonica(nChannel: 4, dAmpl: AnalGr1[2], dPhase: AnalGr1[3], dFreq: 50, dExp: 0);
                            //channels.AddHarmonica(nChannel: 5, dAmpl: AnalGr1[4], dPhase: AnalGr1[5], dFreq: 50, dExp: 0);
                            //channels.AddHarmonica(nChannel: 3, dAmpl: AnalGr3[0], dPhase: 0, dFreq: 100, dExp: 0);
                            //channels.AddHarmonica(nChannel: 4, dAmpl: AnalGr3[1], dPhase: 240, dFreq: 100, dExp: 0);
                            //channels.AddHarmonica(nChannel: 5, dAmpl: AnalGr3[2], dPhase: 120, dFreq: 100, dExp: 0);
                            //channels.AddHarmonica(nChannel: 3, dAmpl: AnalGr4[0], dPhase: 0, dFreq: 250, dExp: 0);
                            //channels.AddHarmonica(nChannel: 4, dAmpl: AnalGr4[1], dPhase: 240, dFreq: 250, dExp: 0);
                            //channels.AddHarmonica(nChannel: 5, dAmpl: AnalGr4[2], dPhase: 120, dFreq: 250, dExp: 0);

                            object ob1 = channels;


                            /*
                                // Используем IRTSineChannels (быстрее)
                                RTLink.IRTSineChannels sig1 = new RTLink.IRTSineChannels()
                            {
                                I = new RTLink.IRTSSignal[3],
                                U = new RTLink.IRTSSignal[3],
                                dFreq = 50.0
                            };

                            sig1.U[0].dAmpl = AnalGr1[6];
                            sig1.U[0].dPhase = AnalGr1[7];
                            sig1.U[1].dAmpl = AnalGr1[8];
                            sig1.U[1].dPhase = AnalGr1[9];
                            sig1.U[2].dAmpl = AnalGr1[10];
                            sig1.U[2].dPhase = AnalGr1[11];

                            sig1.I[0].dAmpl = AnalGr1[0];
                            sig1.I[0].dPhase = AnalGr1[1];
                            sig1.I[1].dAmpl = AnalGr1[2];
                            sig1.I[1].dPhase = AnalGr1[3];
                            sig1.I[2].dAmpl = AnalGr1[4];
                            sig1.I[2].dPhase = AnalGr1[5];

                            object ob1 = sig1;
                            m_nResultReturn = m_retom.Out(ref ob1, RTLink.Constants.RT_UI_ALL);
                            break;
                                */


                            RTDI.CoAnalogOutputs channels1 = m_retom.NewAnalogChannels();
                            channels1.dFrequency = 50;
                            channels1.SetSinSignal(nChannel: 0, dAmpl: AnalGr2[6], dPhase: AnalGr2[7]);
                            channels1.SetSinSignal(nChannel: 1, dAmpl: AnalGr2[8], dPhase: AnalGr2[9]);
                            channels1.SetSinSignal(nChannel: 2, dAmpl: AnalGr2[10], dPhase: AnalGr2[11]);
                            channels1.SetSinSignal(nChannel: 3, dAmpl: AnalGr2[0], dPhase: AnalGr2[1]);
                            channels1.SetSinSignal(nChannel: 4, dAmpl: AnalGr2[2], dPhase: AnalGr2[3]);
                            channels1.SetSinSignal(nChannel: 5, dAmpl: AnalGr2[4], dPhase: AnalGr2[5]);
                            object ob2 = channels1;

                            //m_nResultReturn = m_retom.Enable(); // ПРОВЕРИТЬ РАБОТАЕТ ТАК ИЛИ НЕТ 06.04.2026
                            m_nResultReturn = m_retom.Out61(ref ob1, RTLink.Constants.RT_UI_ALL, ref ob2, RTLink.Constants.RT_UI_ALL);


                            //m_retom.SetTimeOut(0.2);
                            //m_nResultReturn = m_retom.Out(ref ob1, RTLink.Constants.RT_UI_ALL);

                            //var sw = Stopwatch.StartNew();
                            //m_retom.SetTimeOut(0.2);
                            //m_nResultReturn = m_retom.Out(ref ob1, RTLink.Constants.RT_UI_ALL);
                            //sw.Stop();

                            //Debug.WriteLine($"Out выполнен за {sw.ElapsedMilliseconds} мс");
                            //Console.WriteLine($"Out выполнен за {sw.ElapsedMilliseconds} мс");


                            //var info = m_retom.ServerInfo.maxI;
                            //MessageBox.Show(info.ToString());

                            //Thread.Sleep(4000);
                            //channels.AddHarmonica(nChannel: 3, dAmpl: 2, dPhase: 0, dFreq: 100, dExp: 0);
                            //channels.SetSinSignal(3, 2, 0);
                            //channels.AddHarmonica(nChannel: 3, dAmpl: 0.5, dPhase: 0, dFreq: 150, dExp: 0);
                            // m_nResultReturn = m_retom.Out(ref ob11, RTLink.Constants.RT_UI_ALL);
                            //Thread.Sleep(4000);
                            //m_nResultReturn = m_retom.Disable();
                            //object ob12 = channels;
                            //m_nResultReturn = m_retom.Out61(ref ob11, RTLink.Constants.RT_UI_ALL, ref ob12, RTLink.Constants.RT_UI_ALL);
                            //m_nResultReturn = m_retom.Out61(ref ob1, RTLink.Constants.RT_UI_ALL, ref ob2, RTLink.Constants.RT_UI_ALL);
                            break;

                            case "Out61Harms":

                                RTDI.CoAnalogOutputs channelsH = m_retom.NewAnalogChannels();
                                channelsH.dFrequency = 50;
                                channelsH.SetSinSignal(nChannel: 0, dAmpl: AnalGr1[6], dPhase: AnalGr1[7]);
                                channelsH.SetSinSignal(nChannel: 1, dAmpl: AnalGr1[8], dPhase: AnalGr1[9]);
                                channelsH.SetSinSignal(nChannel: 2, dAmpl: AnalGr1[10], dPhase: AnalGr1[11]);
                                channelsH.SetSinSignal(nChannel: 3, dAmpl: AnalGr1[0], dPhase: AnalGr1[1]);
                                channelsH.SetSinSignal(nChannel: 4, dAmpl: AnalGr1[2], dPhase: AnalGr1[3]);
                                channelsH.SetSinSignal(nChannel: 5, dAmpl: AnalGr1[4], dPhase: AnalGr1[5]);

                                channelsH.AddHarmonica(nChannel: 3, dAmpl: AnalGr1[0], dPhase: AnalGr1[1], dFreq: 50, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 4, dAmpl: AnalGr1[2], dPhase: AnalGr1[3], dFreq: 50, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 5, dAmpl: AnalGr1[4], dPhase: AnalGr1[5], dFreq: 50, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 3, dAmpl: AnalGr3[0], dPhase: 0, dFreq: 100, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 4, dAmpl: AnalGr3[1], dPhase: 240, dFreq: 100, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 5, dAmpl: AnalGr3[2], dPhase: 120, dFreq: 100, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 3, dAmpl: AnalGr4[0], dPhase: 0, dFreq: 250, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 4, dAmpl: AnalGr4[1], dPhase: 240, dFreq: 250, dExp: 0);
                                channelsH.AddHarmonica(nChannel: 5, dAmpl: AnalGr4[2], dPhase: 120, dFreq: 250, dExp: 0);

                                object ob1H = channelsH;

                                RTDI.CoAnalogOutputs channels1H = m_retom.NewAnalogChannels();
                                channels1H.dFrequency = 50;
                                channels1H.SetSinSignal(nChannel: 0, dAmpl: AnalGr2[6], dPhase: AnalGr2[7]);
                                channels1H.SetSinSignal(nChannel: 1, dAmpl: AnalGr2[8], dPhase: AnalGr2[9]);
                                channels1H.SetSinSignal(nChannel: 2, dAmpl: AnalGr2[10], dPhase: AnalGr2[11]);
                                channels1H.SetSinSignal(nChannel: 3, dAmpl: AnalGr2[0], dPhase: AnalGr2[1]);
                                channels1H.SetSinSignal(nChannel: 4, dAmpl: AnalGr2[2], dPhase: AnalGr2[3]);
                                channels1H.SetSinSignal(nChannel: 5, dAmpl: AnalGr2[4], dPhase: AnalGr2[5]);
                                object ob2H = channels1H;

                                m_nResultReturn = m_retom.Out61(ref ob1H, RTLink.Constants.RT_UI_ALL, ref ob2H, RTLink.Constants.RT_UI_ALL);

                                break;
                        }
                }
            }
            catch
            {
                m_stError = "Error_RunRetom";
            }
        }

        public bool RemoveRetom()
        {
            bool processKilled = false;
            bool recreationSuccess = false;

            try
            {
                // 1. Поиск и завершение процессов
                Process[] processes = Process.GetProcessesByName("RTDI");
                foreach (Process p in processes)
                {
                    try
                    {
                        if (p.MainModule?.ModuleName == "RTDI.exe")
                        {
                            p.Kill();
                            p.WaitForExit(2000); // Даем время на завершение
                            processKilled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error killing process {p.Id}: {ex.Message}");
                        continue;
                    }
                }

                // 2. Пересоздание соединения
                if (processKilled || processes.Length == 0)
                {
                    recreationSuccess = CreateRetom();
                }

                return recreationSuccess;
            }
            catch (Exception ex)
            {
                m_stError = $"RemoveRetom failed: {ex.Message}";
                Debug.WriteLine(m_stError);
                return false;
            }
            finally
            {
                if (!recreationSuccess)
                {
                    m_stError = "Recreation failed after process termination";
                }
            }
        }



    }
}
