using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ѡ���� һ�����ʹ��
/// </summary>
public class DatePicker : MonoSingletonBase<DatePicker>
{
    public Dropdown yearDropdown;
    public Dropdown monthDropdown;
    public Dropdown dayDropdown;

    //public Button confirmBtn;

    private List<int> years = new List<int>();
    private List<int> months = new List<int>();
    private List<int> days = new List<int>();

    void Start()
    {
        // ��ʼ����ݣ�1900-��ǰ�꣩
        int currentYear = System.DateTime.Now.Year;
        for (int year = currentYear; year >= currentYear - 100; year--)
        {
            years.Add(year);
        }
        PopulateDropdown(yearDropdown, years);
        oldYear = currentYear;

        // ��ʼ���·ݣ�1-12��
        int currentMonth = DateTime.Now.Month;
        for (int month = 1; month <= currentMonth; month++)
        {
            months.Add(month);
        }
        PopulateDropdown(monthDropdown, months);

        // ��ʼ���գ�Ĭ��1-31��������̬���£�
        UpdateDays(1, 1); // Ĭ��1��1��

        yearDropdown.onValueChanged.AddListener(delegate { OnYearOrMonthChanged(); });
        monthDropdown.onValueChanged.AddListener(delegate { OnYearOrMonthChanged(); });
        dayDropdown.onValueChanged.AddListener(delegate { OnDayChanged(); });

        //confirmBtn.onClick.AddListener(() =>
        //{
        //    Debug.Log("ѡ������ڣ�" + GetSelectedDate().ToString("yyyy-MM-dd"));
        //});

        // ����Ĭ��ѡ�е�ǰ����
        System.DateTime now = System.DateTime.Now;
        yearDropdown.value = years.IndexOf(now.Year);
        monthDropdown.value = months.IndexOf(now.Month);
        UpdateDays(now.Year, now.Month);
        dayDropdown.value = days.IndexOf(now.Day);
    }

    // ͨ�÷��������Dropdownѡ��
    private void PopulateDropdown(Dropdown dropdown, List<int> options)
    {
        dropdown.ClearOptions();
        List<string> optionStrings = new List<string>();
        foreach (var option in options)
        {
            optionStrings.Add(option.ToString());
        }
        dropdown.AddOptions(optionStrings);
    }
    // �� DatePicker ����������·���
    private void UpdateDays(int selectedYear, int selectedMonth)
    {
        int daysInMonth;
        if (oldYear == DateTime.Now.Year && selectedMonth == DateTime.Now.Month)
        {
            daysInMonth = DateTime.Now.Day;
        }
        else
        {
            daysInMonth = System.DateTime.DaysInMonth(selectedYear, selectedMonth);
        }
        days.Clear();
        for (int day = 1; day <= daysInMonth; day++)
        {
            days.Add(day);
        }
        PopulateDropdown(dayDropdown, days);
    }

    int oldYear = 0;
    private void OnYearOrMonthChanged()
    {
        int selectedYear = years[yearDropdown.value];
        if (selectedYear == DateTime.Now.Year && oldYear != selectedYear)
        {
            oldYear = selectedYear;
            months.Clear();
            int currentMonth = DateTime.Now.Month;
            for (int month = 1; month <= currentMonth; month++)
            {
                months.Add(month);
            }
            PopulateDropdown(monthDropdown, months);
        }
        else if (oldYear != selectedYear)
        {
            oldYear = selectedYear;
            months.Clear();
            for (int month = 1; month <= 12; month++)
            {
                months.Add(month);
            }
            PopulateDropdown(monthDropdown, months);
        }

        int selectedMonth;

        if (months[monthDropdown.value] <= DateTime.Now.Month)
        {
            selectedMonth = months[monthDropdown.value];
        }
        else
        {
            //months[monthDropdown.value] = DateTime.Now.Month;
            monthDropdown.value = DateTime.Now.Month - 1;
            selectedMonth = DateTime.Now.Month;
        }
        //int selectedDay;
        //if (days[dayDropdown.value] <= DateTime.Now.Day)
        //{
        //    selectedDay = days[dayDropdown.value];
        //    //dayDropdown.value = selectedDay - 1; 
        //}
        //else
        //{
        //    //days[dayDropdown.value] = DateTime.Now.Day;
        //    dayDropdown.value = DateTime.Now.Day - 1;
        //    selectedDay = DateTime.Now.Day;
        //}
        UpdateDays(selectedYear, selectedMonth);
    }
    private void OnDayChanged()
    {
        if (days[dayDropdown.value] >= DateTime.Now.Day)
        {
            dayDropdown.value = DateTime.Now.Day - 1;

        }

    }

    public bool BackYear()
    {
        DateTime now = DateTime.Now.Date;

        DateTime dateTime = new DateTime(years[yearDropdown.value], months[monthDropdown.value], days[dayDropdown.value]);

        DateTime fiveYear = dateTime.AddYears(5);

        bool isYearAgo = true;

        if (now < fiveYear)
        {
            return isYearAgo = false;
        }

        return isYearAgo;
    }



    // ��ӷ�����ȡ��ǰѡ�������
    public System.DateTime GetSelectedDate()
    {
        int year = years[yearDropdown.value];
        int month = months[monthDropdown.value];
        int day = days[dayDropdown.value];
        return new System.DateTime(year, month, day);
    }


}
