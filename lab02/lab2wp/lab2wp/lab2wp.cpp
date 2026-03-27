#include <string>
#include <memory>
#include <cstdio>
#include <cwchar>


#include <windows.h>
#include <commctrl.h>

#pragma comment(lib, "comctl32.lib")

// Реализация: Платёжные шлюзы
class PaymentGateway {
public:
    virtual ~PaymentGateway() = default;
    virtual bool process(double amount) = 0;
    virtual const char* name() const = 0;
    virtual double fee() const = 0;
};

class Stripe : public PaymentGateway {
public:
    bool process(double amount) override { return true; }
    const char* name() const override { return "Stripe"; }
    double fee() const override { return 2.9; }
};

class PayPal : public PaymentGateway {
public:
    bool process(double amount) override { return true; }
    const char* name() const override { return "PayPal"; }
    double fee() const override { return 3.4; }
};

class Bank : public PaymentGateway {
public:
    bool process(double amount) override { return true; }
    const char* name() const override { return "Банк"; }
    double fee() const override { return 1.5; }
};

// Абстракция: Способы оплаты (Плохая реализация без моста)
class PaymentMethod {
public:
    virtual ~PaymentMethod() = default;
    virtual bool pay(double amount) = 0;
    virtual const char* type() const = 0;
    virtual double calcFee(double amount) = 0;
};

// Комбинация: Карта + Stripe
class CardPay : public PaymentMethod {
public:
    bool pay(double amount) override { return true; }
    const char* type() const override { return "Карта"; }
    double calcFee(double amount) override {
        return amount * 2.9 / 100.0;
    }
};

// Комбинация: Карта + PayPal
class CardPayPal : public PaymentMethod {
public:
    bool pay(double amount) override { return true; }
    const char* type() const override { return "Карта"; }
    double calcFee(double amount) override {
        return amount * 3.4 / 100.0;
    }
};

// Комбинация: Карта + Банк
class CardBank : public PaymentMethod {
public:
    bool pay(double amount) override { return true; }
    const char* type() const override { return "Карта"; }
    double calcFee(double amount) override {
        return amount * 1.5 / 100.0;
    }
};

// Комбинация: Крипта + Stripe
class CryptoPay : public PaymentMethod {
public:
    bool pay(double amount) override { return true; }
    const char* type() const override { return "Крипта"; }
    double calcFee(double amount) override {
        return amount * 2.9 / 100.0;
    }
};

// Комбинация: Крипта + PayPal
class CryptoPayPal : public PaymentMethod {
public:
    bool pay(double amount) override { return true; }
    const char* type() const override { return "Крипта"; }
    double calcFee(double amount) override {
        return amount * 3.4 / 100.0;
    }
};

// Комбинация: Крипта + Банк
class CryptoBank : public PaymentMethod {
public:
    bool pay(double amount) override { return true; }
    const char* type() const override { return "Крипта"; }
    double calcFee(double amount) override {
        return amount * 1.5 / 100.0;
    }
};

HWND hWnd, hComboGW, hComboMethod, hEditAmount, hTxtResult;
std::unique_ptr<PaymentMethod> payment;
int gwIdx = 0, methodIdx = 0;


double safeStod(const wchar_t* wstr, double def = 100.0) {
    try {
        // Конвертация wchar_t* в string
        int len = WideCharToMultiByte(CP_ACP, 0, wstr, -1, nullptr, 0, nullptr, nullptr);
        if (len <= 1) return def;
        std::string str(len - 1, '\0');
        WideCharToMultiByte(CP_ACP, 0, wstr, -1, &str[0], len, nullptr, nullptr);

        if (str.empty()) return def;
        double val = std::stod(str);
        return (val > 0) ? val : def;
    }
    catch (...) {
        return def;
    }
}

void updatePayment() {
    if (methodIdx == 0) { // Карта
        if (gwIdx == 0) payment = std::make_unique<CardPay>();
        else if (gwIdx == 1) payment = std::make_unique<CardPayPal>();
        else if (gwIdx == 2) payment = std::make_unique<CardBank>();
    }
    else if (methodIdx == 1) { // Крипта
        if (gwIdx == 0) payment = std::make_unique<CryptoPay>();
        else if (gwIdx == 1) payment = std::make_unique<CryptoPayPal>();
        else if (gwIdx == 2) payment = std::make_unique<CryptoBank>();
    }
}

void doPayment() {
    wchar_t buf[50];
    GetWindowTextW(hEditAmount, buf, 50);
    double amount = safeStod(buf);

    if (!payment) return;

    double fee = payment->calcFee(amount);

    char result[256];
    if (payment->pay(amount)) {
        sprintf_s(result,
            "УСПЕХ: $%.2f + комиссия $%.2f через %s",
            amount, fee, payment->type());
    }
    else {
        sprintf_s(result, "ОШИБКА!");
    }
    SetWindowTextA(hTxtResult, result);
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT msg, WPARAM wp, LPARAM lp) {
    if (msg == WM_CREATE) {
        // Инициализация контролов
        INITCOMMONCONTROLSEX icex = {};
        icex.dwSize = sizeof(icex);
        icex.dwICC = ICC_STANDARD_CLASSES;
        InitCommonControlsEx(&icex);
        return 0;
    }

    if (msg == WM_COMMAND) {
        if (LOWORD(wp) == 1 && HIWORD(wp) == CBN_SELCHANGE) {
            gwIdx = (int)SendMessageA(hComboGW, CB_GETCURSEL, 0, 0);
            updatePayment();
        }
        if (LOWORD(wp) == 2 && HIWORD(wp) == CBN_SELCHANGE) {
            methodIdx = (int)SendMessageA(hComboMethod, CB_GETCURSEL, 0, 0);
            updatePayment();
        }
        if (LOWORD(wp) == 10) doPayment();
        if (LOWORD(wp) == 11) SetWindowTextA(hTxtResult, "Возврат не поддерживается");
    }

    if (msg == WM_DESTROY) {
        PostQuitMessage(0);
        return 0;
    }
    return DefWindowProcA(hWnd, msg, wp, lp);
}

void createUI(HWND hWnd) {
    HFONT font = CreateFontA(14, 0, 0, 0, FW_NORMAL, 0, 0, 0,
        RUSSIAN_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
        DEFAULT_QUALITY, DEFAULT_PITCH | FF_DONTCARE, "Segoe UI");

    CreateWindowA("STATIC", "Платёжный шлюз:", WS_CHILD | WS_VISIBLE | SS_RIGHT,
        20, 20, 110, 20, hWnd, 0, 0, 0);

    hComboGW = CreateWindowA("COMBOBOX", "",
        WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
        140, 18, 180, 100, hWnd, (HMENU)1, nullptr, nullptr);
    SendMessageA(hComboGW, CB_ADDSTRING, 0, (LPARAM)"Stripe");
    SendMessageA(hComboGW, CB_ADDSTRING, 0, (LPARAM)"PayPal");
    SendMessageA(hComboGW, CB_ADDSTRING, 0, (LPARAM)"Банк");
    SendMessageA(hComboGW, CB_SETCURSEL, 0, 0);

    CreateWindowA("STATIC", "Способ оплаты:", WS_CHILD | WS_VISIBLE | SS_RIGHT,
        20, 60, 110, 20, hWnd, 0, 0, 0);

    hComboMethod = CreateWindowA("COMBOBOX", "",
        WS_CHILD | WS_VISIBLE | CBS_DROPDOWNLIST | WS_VSCROLL,
        140, 58, 180, 100, hWnd, (HMENU)2, nullptr, nullptr);

    
    SendMessageA(hComboMethod, CB_ADDSTRING, 0, (LPARAM)"[CARD] Карта");
    SendMessageA(hComboMethod, CB_ADDSTRING, 0, (LPARAM)"[CRYPTO] Крипта");

    SendMessageA(hComboMethod, CB_SETCURSEL, 0, 0);

    CreateWindowA("STATIC", "Сумма ($):", WS_CHILD | WS_VISIBLE | SS_RIGHT,
        20, 100, 80, 20, hWnd, 0, 0, 0);

    hEditAmount = CreateWindowA("EDIT", "100",
        WS_CHILD | WS_VISIBLE | WS_BORDER | ES_NUMBER | ES_CENTER,
        100, 98, 100, 25, hWnd, (HMENU)3, 0, 0);

    
    CreateWindowA("BUTTON", "[PAY] ОПЛАТИТЬ",
        WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON | BS_FLAT,
        20, 140, 120, 35, hWnd, (HMENU)10, 0, 0);

    CreateWindowA("BUTTON", "[REFUND] ВОЗВРАТ",
        WS_CHILD | WS_VISIBLE | BS_PUSHBUTTON | BS_FLAT,
        150, 140, 120, 35, hWnd, (HMENU)11, 0, 0);

    hTxtResult = CreateWindowA("STATIC", "",
        WS_CHILD | WS_VISIBLE | SS_CENTER | SS_NOTIFY,
        20, 190, 400, 30, hWnd, 0, 0, 0);

    HWND controls[] = { hComboGW, hComboMethod, hEditAmount, hTxtResult };
    for (HWND ctrl : controls) {
        if (ctrl) SendMessageA(ctrl, WM_SETFONT, (WPARAM)font, TRUE);
    }

    updatePayment();
}

int WINAPI WinMain(HINSTANCE hInst, HINSTANCE, LPSTR, int nShow) {
    WNDCLASSEXA wc = {};
    wc.cbSize = sizeof(wc);
    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInst;
    wc.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wc.lpszClassName = "NoBridgeApp";
    wc.hIcon = LoadIcon(nullptr, IDI_APPLICATION);
    wc.hIconSm = LoadIcon(nullptr, IDI_APPLICATION);

    if (!RegisterClassExA(&wc)) {
        MessageBoxA(nullptr, "Ошибка регистрации окна", "Error", MB_ICONERROR);
        return 0;
    }

    hWnd = CreateWindowExA(
        WS_EX_CLIENTEDGE,
        "NoBridgeApp",
        "Без паттерна МОСТ", 
        WS_OVERLAPPEDWINDOW & ~WS_THICKFRAME & ~WS_MAXIMIZEBOX,
        100, 100, 500, 320,
        nullptr, nullptr, hInst, nullptr
    );

    if (!hWnd) {
        MessageBoxA(nullptr, "Ошибка создания окна", "Error", MB_ICONERROR);
        return 0;
    }

    ShowWindow(hWnd, nShow);
    UpdateWindow(hWnd);
    createUI(hWnd);

    MSG msg;
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
    return (int)msg.wParam;
}