// Blazor JavaScript interop functions for Mega Able theme

// Custom error handler to intercept Blazor errors
window.Blazor = window.Blazor || {};

// Store the original error UI display function
let originalShowErrorNotification;

// Wait for Blazor to load and then override its error handling
document.addEventListener('DOMContentLoaded', function() {
    // Hide any error UI elements that might exist
    setInterval(function() {
        const errorUi = document.getElementById('blazor-error-ui');
        if (errorUi) {
            errorUi.style.display = 'none';
            errorUi.style.visibility = 'hidden';
            errorUi.style.opacity = '0';
        }
    }, 100);
    
    // Override Blazor's error notification system when it becomes available
    if (window.Blazor) {
        overrideBlazorErrors();
    } else {
        // Wait for Blazor to initialize
        const checkBlazor = setInterval(function() {
            if (window.Blazor) {
                clearInterval(checkBlazor);
                overrideBlazorErrors();
            }
        }, 100);
    }
});

// Function to override Blazor's error handling
function overrideBlazorErrors() {
    if (window.Blazor._internal) {
        if (window.Blazor._internal.showErrorNotification) {
            originalShowErrorNotification = window.Blazor._internal.showErrorNotification;
            window.Blazor._internal.showErrorNotification = function(message) {
                // Log the error but don't show the UI
                console.log('Blazor error intercepted:', message);
                return false;
            };
        }
    }
}

// Override the default error handler
window.addEventListener('error', function (e) {
    // Prevent default error handling
    e.preventDefault();
    e.stopPropagation();
    console.log('Error intercepted:', e.error || e.message);
    return true;
});

// Suppress unhandled promise rejections
window.addEventListener('unhandledrejection', function (e) {
    // Prevent default error handling
    e.preventDefault();
    e.stopPropagation();
    console.log('Unhandled promise rejection intercepted:', e.reason);
    return true;
});
window.initTheme = function () {
    // Initialize pcoded menu when DOM is ready
    $(document).ready(function() {
        if ($("#pcoded").length) {
            // Reinitialize the pcoded menu
            $("#pcoded").pcodedmenu({
                themelayout: 'vertical',
                verticalMenuplacement: 'left',
                verticalMenulayout: 'wide',
                MenuTrigger: 'click',
                SubMenuTrigger: 'click',
                activeMenuClass: 'active',
                ThemeBackgroundPattern: 'theme1',
                HeaderBackground: 'theme1',
                LHeaderBackground: 'theme1',
                NavbarBackground: 'themelight1',
                ActiveItemBackground: 'theme1',
                SubItemBackground: 'theme2',
                ActiveItemStyle: 'style0',
                ItemBorder: true,
                ItemBorderStyle: 'none',
                NavbarImage: 'false',
                ActiveNavbarImage: 'img1',
                SubItemBorder: true,
                DropDownIconStyle: 'style3',
                menutype: 'st2',
                layouttype: 'light',
                FixedNavbarPosition: true,
                FixedHeaderPosition: true,
                collapseVerticalLeftHeader: true,
                VerticalSubMenuItemIconStyle: 'style7',
                VerticalNavigationView: 'view1',
                verticalMenueffect: {
                    desktop: "shrink",
                    tablet: "overlay",
                    phone: "overlay",
                },
                defaultVerticalMenu: {
                    desktop: "expanded",
                    tablet: "offcanvas",
                    phone: "offcanvas",
                },
                onToggleVerticalMenu: {
                    desktop: "offcanvas",
                    tablet: "expanded",
                    phone: "expanded",
                },
            });
        }

        // Initialize scrollbars
        if ($.mCustomScrollbar) {
            $(".main-menu").mCustomScrollbar({
                setTop: "1px",
                setHeight: "calc(100% - 56px)",
                autoHideScrollbar: false,
                scrollInertia: 100,
                theme: "minimal",
            });
        }

        // Make sure sidebar toggle works
        $(".sidebar_toggle a").on("click", function() {
            $(".pcoded-navbar").toggleClass("hide-sidebar");
            if ($(".pcoded-navbar").hasClass("hide-sidebar")) {
                $(".pcoded-main-container").css('margin-left', '0');
            } else {
                $(".pcoded-main-container").css('margin-left', '');
            }
        });

        // Mobile menu toggle
        $(".mobile-options").on("click", function() {
            $(".navbar-container .nav-right").toggleClass("show-menu");
        });
        
        // Initialize profile dropdown
        $(".header-notification").on("click", function() {
            $(this).find(".show-notification").slideToggle(300);
            $(this).toggleClass("active");
        });
        
        // Close dropdown when clicking outside
        $(document).on("click", function(event) {
            var $trigger = $(".header-notification");
            if($trigger !== event.target && !$trigger.has(event.target).length) {
                $(".show-notification").slideUp(300);
                $(".header-notification").removeClass("active");
            }
        });
    });
};

// Toggle sidebar from Blazor
window.toggleSidebar = function () {
    $(".sidebar_toggle a").trigger("click");
};

// Toggle mobile menu from Blazor
window.toggleMobileMenu = function () {
    $(".mobile-options").trigger("click");
};
