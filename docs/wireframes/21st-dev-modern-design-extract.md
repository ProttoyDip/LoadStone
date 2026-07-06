# 21st.dev Modern Design Extract

Source reviewed: <https://21st.dev/> and its public component category pages.

This document extracts reusable UI/UX patterns from 21st.dev for Lodestone. Treat these as inspiration and design-system direction, not as copied components, copied code, or exact visual replication.

## What 21st.dev Is Doing Well

- Uses a component-first product model: users browse by practical UI categories such as heroes, backgrounds, features, buttons, inputs, cards, tables, tabs, sidebars, dialogs, and AI chat components.
- Makes search and filtering central to navigation, including a visible command-key shortcut pattern.
- Separates marketing blocks from application UI components, which keeps browsing intent clear.
- Uses modern motion-heavy patterns such as scroll animation, shader backgrounds, spotlight cards, orbital timelines, animated text, and media expansion heroes.
- Presents components as compact visual previews with creator identity and popularity signals.
- Uses dense category navigation without overwhelming the page because categories are grouped by purpose.

## Patterns To Adapt For Lodestone

### 1. Command Search And Fast Navigation

Use a command/search entry in the authenticated app shell for quick access to students, risk queue, journal entries, bookings, forum posts, reports, and crisis resources.

Design notes:

- Place the search affordance in the top bar.
- Show a `Ctrl+K` style shortcut label.
- Group results by object type.
- Keep actions contextual: view student, open booking, review risk item, moderate forum post.

Best Lodestone targets:

- Counselor dashboard
- Admin dashboard
- Risk queue
- Forum moderation

### 2. Category-Based Browsing

21st.dev groups components into meaningful categories instead of a flat list. Lodestone can use the same information architecture for support resources and dashboards.

Design notes:

- Group crisis resources by urgency, audience, and support type.
- Group forum categories with counts and activity indicators.
- Group reports by operational use: risk, engagement, counselor sessions, forum health.

Best Lodestone targets:

- Crisis resource page
- Forum index
- Reports page
- Admin dashboard

### 3. Compact Preview Cards

Use card previews for repeated items, but keep them functional and restrained. Each card should expose status, owner, recency, and the next action.

Design notes:

- Use cards for risk queue students, counselor availability slots, forum categories, and report templates.
- Keep the card radius at or below `8px`.
- Avoid nested cards.
- Add one strong action and one secondary action at most.

Best Lodestone targets:

- Counselor queue
- Booking page
- Forum categories
- Student dashboard

### 4. Bento-Style Summary Layouts

Use asymmetric summary grids for dashboard overviews, where the most important metric gets more space and supporting metrics stay compact.

Design notes:

- Use one large panel for the primary workflow, such as high-risk queue or today bookings.
- Use smaller panels for mood trends, nudges sent, forum activity, and engagement changes.
- Keep the grid dense enough for professional use.

Best Lodestone targets:

- Admin dashboard
- Counselor dashboard
- Student dashboard

### 5. Motion With Purpose

21st.dev highlights animation patterns heavily, but Lodestone should use motion sparingly because this is a wellbeing product.

Good uses:

- Subtle hover states on cards and buttons.
- Smooth panel expansion for filters and details.
- Lightweight progress transitions for onboarding/check-ins.
- Non-alarming pulse only for live queue updates.

Avoid:

- Distracting shader backgrounds on sensitive workflows.
- Flashing, intense glow, or urgent red animation around student risk.
- Excessive parallax in counselor or crisis views.

### 6. Modern Button And Input Treatment

Use polished controls without making the interface feel like a demo gallery.

Design notes:

- Buttons should have clear hierarchy: primary, secondary, ghost, destructive.
- Inputs should have clear labels, validation text, and stable focus rings.
- Search inputs should feel prominent on listing pages.
- Icon-only buttons need accessible names and tooltips.

Best Lodestone targets:

- Booking form
- Journal form
- Forum composer
- Dashboard filters

### 7. Rich Empty States

21st.dev treats component previews as browseable objects. Lodestone can use similar visual affordance for empty states, but with empathetic, low-pressure copy.

Design notes:

- Empty risk queue: reinforce that no immediate review is needed.
- Empty journal: invite optional reflection without pressure.
- Empty forum category: invite a first post with community guidelines.
- Empty bookings: show availability and a clear booking path.

### 8. Popularity And Status Signals

21st.dev displays popularity and creator signals. Lodestone can adapt this as operational metadata, not social proof.

Useful signals:

- Risk level
- Last activity
- Assigned counselor
- Booking status
- Forum moderation status
- Report generation status
- Nudge status

## Visual Direction For Lodestone

Use a calm professional interface rather than a flashy marketplace aesthetic.

Recommended tone:

- Neutral base surfaces with a soft blue/green accent system.
- Clear status colors for low, medium, high, and urgent states.
- Rounded but not pill-heavy UI.
- Subtle borders instead of heavy shadows.
- Dense dashboard layouts with strong scanability.
- Motion that communicates state changes, not decoration.

Avoid:

- Full-page shader backgrounds.
- Neon buttons for clinical or wellbeing flows.
- Overuse of gradients.
- Decorative floating shapes.
- Marketing-style hero pages inside authenticated tools.

## Concrete Component Ideas

### App Shell

- Left sidebar for primary modules: Dashboard, Risk Queue, Journal, Forum, Booking, Crisis Resources, Reports, Admin.
- Top bar with command search, notifications, role switch context, and account menu.
- Breadcrumbs on deep operational pages.

### Risk Queue

- Priority list with compact rows or cards.
- Filters for risk level, assigned counselor, last activity, and status.
- Detail drawer for student context.
- Primary action: assign or review.
- Secondary action: send nudge, book session, export summary.

### Student Dashboard

- Bento grid with wellbeing check-in, recent nudges, upcoming booking, journal prompt, forum entry points, and crisis resource access.
- Keep crisis access visible but not visually alarming.

### Forum

- Category grid with activity counts and moderation status.
- Post list with tags, status badges, and last activity.
- Composer with clear privacy/moderation guidance.

### Booking

- Calendar/list hybrid.
- Availability cards with counselor, date, time, mode, and status.
- Confirmation state with next steps.

### Reports

- Template gallery using compact preview cards.
- Status badges for generated, pending, failed.
- Filters by student, date range, report type, and counselor.

## Implementation Notes

- Build on the existing Razor views and CSS files first.
- Introduce shared CSS tokens in `src/Lodestone.Web/wwwroot/css/site.css`.
- Create reusable utility classes for cards, badges, buttons, inputs, page headers, empty states, and dashboard grids.
- Keep page-specific CSS in existing files such as `dashboard.css`, `forum.css`, and `landing.css`.
- Verify desktop and mobile layouts after each major page redesign.

## Source Notes

- 21st.dev homepage positions the product around browsing thousands of community components and templates with full filters.
- The component directory separates marketing blocks from UI components.
- Public categories include heroes, backgrounds, features, shaders, buttons, inputs, cards, tables, tabs, sidebars, dialogs, and AI chat components.
- Popular examples shown publicly include scroll animation heroes, Spline scenes, spotlight cards, radial timelines, bento grids, shader backgrounds, liquid/glass buttons, and animated text.
