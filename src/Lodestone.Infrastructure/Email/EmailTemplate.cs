namespace Lodestone.Infrastructure.Email;

/// <summary>
/// Wraps any HTML body content in the Lodestone branded email shell.
/// Colours and typography match the landing page design tokens.
/// </summary>
public static class EmailTemplate
{
    // ── Design tokens (mirrored from landing.css) ─────────────────────────
    private const string Paper     = "#F4EEE3";   // warm ivory background
    private const string Paper2    = "#EDE5D6";   // slightly deeper panel
    private const string Card      = "#FBF7EF";   // raised card
    private const string Ink       = "#201C17";   // near-black warm
    private const string InkSoft   = "#4A433A";   // body secondary
    private const string MutedColor = "#857B6C";   // captions
    private const string Clay      = "#BC5138";   // terracotta accent
    private const string ClayDeep  = "#9A3F2A";   // hover clay
    private const string Forest    = "#3C5647";   // botanical green
    private const string Espresso  = "#201C17";   // dark footer

    /// <summary>
    /// Wraps <paramref name="innerHtml"/> in the full branded shell.
    /// <paramref name="previewText"/> is the inbox snippet (hidden in email body).
    /// </summary>
    public static string Wrap(string innerHtml, string previewText = "A message from Lodestone")
    {
        return $$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="UTF-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1.0" />
          <meta http-equiv="X-UA-Compatible" content="IE=edge" />
          <title>Lodestone</title>
          <!--[if mso]>
          <noscript>
            <xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml>
          </noscript>
          <![endif]-->
          <style>
            @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600&display=swap');
            * { box-sizing: border-box; margin: 0; padding: 0; }
            body { background-color: {{Paper}}; font-family: 'Inter', Arial, sans-serif; color: {{Ink}}; }
            a { color: {{Clay}}; text-decoration: none; }
            a:hover { color: {{ClayDeep}}; text-decoration: underline; }
          </style>
        </head>
        <body style="margin:0;padding:0;background-color:{{Paper}};">

          <!-- Preview text (hidden) -->
          <div style="display:none;max-height:0;overflow:hidden;mso-hide:all;">
            {{previewText}}&nbsp;&#847;&nbsp;
          </div>

          <!-- Outer wrapper -->
          <table width="100%" cellpadding="0" cellspacing="0" border="0"
                 style="background-color:{{Paper}};background-image:radial-gradient(rgba(32,28,23,0.035) 1px,transparent 1px);background-size:22px 22px;padding:32px 16px;">
            <tr>
              <td align="center">

                <!-- Card container -->
                <table width="100%" cellpadding="0" cellspacing="0" border="0"
                       style="max-width:580px;background-color:{{Card}};border-radius:10px;border:1px solid rgba(32,28,23,0.12);box-shadow:6px 6px 0 rgba(32,28,23,0.07);overflow:hidden;">

                  <!-- Header / brand bar -->
                  <tr>
                    <td style="background-color:{{Espresso}};padding:22px 36px;">
                      <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                          <td>
                            <span style="font-family:'Inter',Arial,sans-serif;font-size:22px;font-weight:700;color:{{Paper}};letter-spacing:-0.02em;">
                              Lodestone<span style="color:{{Clay}};">.</span>
                            </span>
                          </td>
                          <td align="right">
                            <span style="font-family:'Inter',Arial,sans-serif;font-size:11px;font-weight:500;color:#A79C8A;letter-spacing:0.08em;text-transform:uppercase;">
                              Student Wellbeing
                            </span>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Divider line (clay) -->
                  <tr>
                    <td style="height:3px;background:linear-gradient(90deg,{{Clay}} 0%,{{Forest}} 100%);"></td>
                  </tr>

                  <!-- Body content -->
                  <tr>
                    <td style="padding:36px 36px 28px;font-family:'Inter',Arial,sans-serif;font-size:15px;line-height:1.7;color:{{InkSoft}};">
                      {{innerHtml}}
                    </td>
                  </tr>

                  <!-- Divider -->
                  <tr>
                    <td style="padding:0 36px;">
                      <div style="border-top:1px solid rgba(32,28,23,0.14);"></div>
                    </td>
                  </tr>

                  <!-- Footer -->
                  <tr>
                    <td style="padding:20px 36px 28px;font-family:'Inter',Arial,sans-serif;font-size:12px;color:{{MutedColor}};line-height:1.6;">
                      <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                          <td>
                            <p style="margin:0 0 4px;">
                              You received this email because you have a Lodestone account.
                            </p>
                            <p style="margin:0;">
                              &copy; Lodestone &mdash; Student Wellbeing Platform
                            </p>
                          </td>
                          <td align="right" valign="top">
                            <span style="display:inline-block;width:28px;height:28px;background-color:{{Clay}};border-radius:50%;text-align:center;line-height:28px;font-family:'Inter',Arial,sans-serif;font-size:13px;font-weight:700;color:#fff;">L</span>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                </table>
                <!-- /Card container -->

              </td>
            </tr>
          </table>

        </body>
        </html>
        """;
    }

    // ── Pre-built button helper ───────────────────────────────────────────
    public static string Button(string url, string label) =>
        $"""
        <p style="margin:24px 0;">
          <a href="{url}"
             style="display:inline-block;padding:12px 28px;background-color:#BC5138;color:#FBF7EF;font-family:'Inter',Arial,sans-serif;font-size:14px;font-weight:600;border-radius:6px;text-decoration:none;letter-spacing:0.01em;">
            {label}
          </a>
        </p>
        """;

    // ── Heading helper ────────────────────────────────────────────────────
    public static string Heading(string text) =>
        $"""<h2 style="font-family:'Inter',Arial,sans-serif;font-size:20px;font-weight:700;color:#201C17;margin:0 0 16px;line-height:1.3;">{text}</h2>""";

    // ── Paragraph helper ─────────────────────────────────────────────────
    public static string Para(string text) =>
        $"""<p style="margin:0 0 14px;color:#4A433A;font-size:15px;line-height:1.7;">{text}</p>""";

    // ── Muted small text ─────────────────────────────────────────────────
    public static string SmallMuted(string text) =>
        $"""<p style="margin:14px 0 0;font-size:13px;color:#857B6C;line-height:1.6;">{text}</p>""";
}
