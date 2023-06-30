clickd_jquery(document).ready(function () {
  clickd_jquery(window).on("load", function () {
    clickd_jquery(
      "span:contains('If yes, please provide your Primary/Previous Association.')"
    ).hide();
    clickd_jquery("#cont_id_f_26eac3645fece9119c3000155d10128e").hide();
    clickd_jquery(
      "span:contains('If other, please provide association name.')"
    ).hide();
    clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").hide();
    clickd_jquery(
      "span:contains('If you transferred boards, which board have you moved to?')"
    ).hide();
    clickd_jquery("#cont_id_f_6a6bacd1862dea119c3100155d10128e").hide();
    clickd_jquery(
      "span:contains('If you have a NRDS ID, please enter it here')"
    ).hide();
    clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").hide();
    clickd_jquery(
      "span:contains('If you are transferring board, which board are you moving from?')"
    ).hide();
    const requireAssociation = document.getElementById(
      "cont_id_f_6a6bacd1862dea119c3100155d10128e"
    );
    const requireOtherAssociation = document.getElementById(
      "cont_id_f_af9bfe3360ece9119c3000155d10128e"
    );
    // Show Secondary Questions
    clickd_jquery("#f_7931698eeca3e9119c2700155d10128e").on(
      "change",
      function () {
        if (this.value === "0") {
          alert(
            "You may only apply for secondary membership if you are a member of an local association under the National Association of REALTORS®. You will need to put in your Primary Association information when applying."
          );
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").show();
          clickd_jquery("#cont_id_f_26eac3645fece9119c3000155d10128e").show();
          clickd_jquery(
            "span:contains('If yes, please provide your Primary/Previous Association.')"
          ).show();
          clickd_jquery(
            "span:contains('If you have a NRDS ID, please enter it here')"
          ).show();
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").show();
          clickd_jquery(
            "span:contains('Are you transferring from another Association?')"
          ).hide();
          clickd_jquery("#cont_id_f_d91f4f4ba61eeb119c5500155d10106c").hide();
          clickd_jquery(
            "span:contains('If you transferred boards, which board have you moved to?')"
          ).hide();
          clickd_jquery("#cont_id_f_6a6bacd1862dea119c3100155d10128e").hide();
          clickd_jquery("#f_6a6bacd1862dea119c3100155d10128e").val("");
          clickd_jquery("#f_f01046428c42e7119c1300155d10061b").val("");
        } else if (this.value === "1") {
          clickd_jquery("#f_d91f4f4ba61eeb119c5500155d10106c").val("1");
          clickd_jquery("#f_f01046428c42e7119c1300155d10061b").val("");
          clickd_jquery("#f_26eac3645fece9119c3000155d10128e").val("");
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").hide();
          clickd_jquery("#cont_id_f_26eac3645fece9119c3000155d10128e").hide();
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").hide();
          clickd_jquery(
            "span:contains('If yes, please provide your Primary/Previous Association.')"
          ).hide();
          clickd_jquery(
            "span:contains('If other, please provide association name. ')"
          ).hide();
          clickd_jquery(
            "span:contains('If you have a NRDS ID, please enter it here')"
          ).hide();
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").hide();
          clickd_jquery(
            "span:contains('Are you transferring from another Association?')"
          ).show();
          clickd_jquery("#cont_id_f_d91f4f4ba61eeb119c5500155d10106c").show();
        } else {
          clickd_jquery("#f_26eac3645fece9119c3000155d10128e").val("");
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").hide();
          clickd_jquery("#cont_id_f_26eac3645fece9119c3000155d10128e").hide();
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").hide();
          clickd_jquery(
            "span:contains('If yes, please provide your Primary/Previous Association.')"
          ).hide();
          clickd_jquery(
            "span:contains('If other, please provide association name. ')"
          ).hide();
          clickd_jquery(
            "span:contains('If you have a NRDS ID, please enter it here')"
          ).hide();
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").hide();
          clickd_jquery(
            "span:contains('Are you transferring from another Association?')"
          ).hide();
          clickd_jquery("#cont_id_f_d91f4f4ba61eeb119c5500155d10106c").hide();
        }
      }
    );
    // Show Transfer Questions
    clickd_jquery("#f_d91f4f4ba61eeb119c5500155d10106c").on(
      "change",
      function () {
        if (this.value === "0") {
          clickd_jquery("#cont_id_f_6a6bacd1862dea119c3100155d10128e").show();
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").show();
          clickd_jquery(
            "span:contains('If you transferred boards, which board have you moved to?')"
          ).show();
          clickd_jquery(
            "span:contains('If you have a NRDS ID, please enter it here')"
          ).show();
          clickd_jquery("#nrdsidlookup").show();
          requireAssociation.setAttribute("required", "");
        } else if (this.value === "1") {
          clickd_jquery("#f_6a6bacd1862dea119c3100155d10128e").val("");
          clickd_jquery("#f_af9bfe3360ece9119c3000155d10128e").val("");
          clickd_jquery("#f_f01046428c42e7119c1300155d10061b").val("");
          clickd_jquery("#cont_id_f_6a6bacd1862dea119c3100155d10128e").hide();
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").hide();
          clickd_jquery("#cont_id_f_f01046428c42e7119c1300155d10061b").hide();
          clickd_jquery(
            "span:contains('If you transferred boards, which board have you moved to?')"
          ).hide();
          clickd_jquery(
            "span:contains('If you have a NRDS ID, please enter it here')"
          ).hide();
          clickd_jquery(
            "span:contains('If other, please provide association name.')"
          ).hide();
          clickd_jquery("#nrdsidlookup").hide();
          requireAssociation.removeAttribute("required");
        }
      }
    );
    clickd_jquery("#f_26eac3645fece9119c3000155d10128e").on(
      "change",
      function () {
        if (this.value !== "16") {
          clickd_jquery("#f_af9bfe3360ece9119c3000155d10128e").val("");
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").hide();
          clickd_jquery(
            "span:contains('If other, please provide association name.')"
          ).hide();
        } else {
          clickd_jquery("#f_af9bfe3360ece9119c3000155d10128e").val("");
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").show();
          clickd_jquery(
            "span:contains('If other, please provide association name.')"
          ).show();
        }
      }
    );
    clickd_jquery("#f_6a6bacd1862dea119c3100155d10128e").on(
      "change",
      function () {
        if (this.value !== "Other") {
          requireOtherAssociation.removeAttribute("required");
          clickd_jquery("#f_af9bfe3360ece9119c3000155d10128e").val("");
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").hide();
          clickd_jquery(
            "span:contains('If other, please provide association name.')"
          ).hide();
        } else {
          requireOtherAssociation.setAttribute("required", "");
          clickd_jquery("#cont_id_f_af9bfe3360ece9119c3000155d10128e").show();
          clickd_jquery(
            "span:contains('If other, please provide association name.')"
          ).show();
        }
      }
    );
  });
  // Used to Add Requirement for Upload.
  var targetDiv = clickd_jquery(
    'div.alignBottom.minSize1:has(span:contains("If you are transferring your membership, please upload your Letter of Good Standing."))'
  );
  var nrdsWarning = clickd_jquery('div.alignBottom.minSize3:has(span:contains("If you have a NRDS ID, please enter it here"))');
  var nextButton = document.querySelector(".WizardNextButton");
  if (targetDiv.length > 0) {
    // Create a new <span> element
    var uploadWarning = clickd_jquery("<span>")
      .addClass("maxSize1 requiredInfo")
      .css({
        "font-family": "Verdana",
        "font-weight": "normal",
        "font-size": "12px",
        color: "#ff0000",
      })
      .text("Please upload file before moving forward.");

    var nrdsIdWarning = clickd_jquery("<span>")
    .addClass("maxSize1 requiredInfo")
    .css({
      "font-family": "Verdana",
      "font-weight": "normal",
      "font-size": "12px",
      color: "#ff0000",
    }).text("Please provide nrds ID here");

    var nrdsIdWarningDiv = clickd_jquery("<div>").addClass("maxSize1 requiredInfo").append(nrdsIdWarning);

    // Append the new span to the target div
    clickd_jquery("#f_7931698eeca3e9119c2700155d10128e").on(
      "change",
      function () {
        if (this.value == 0) {
          if (
            clickd_jquery("#f_upload_ab042de5ae80edfdcf8f9de2d905a1dc")[0].files
              .length === 0
          ) {
            targetDiv.append(uploadWarning);
            nextButton.disabled = true;
          } else {
            targetDiv.find("span.maxSize1.requiredInfo").remove();
            nextButton.disabled = false;
          }
        } else {
          targetDiv.find("span.maxSize1.requiredInfo").remove();
          nextButton.disabled = false;
        }
      }
    );
    clickd_jquery("#f_d91f4f4ba61eeb119c5500155d10106c").on(
      "change",
      function () {
        if (this.value == 0) {
          if (
            clickd_jquery("#f_upload_ab042de5ae80edfdcf8f9de2d905a1dc")[0].files
              .length === 0
          ) {
            targetDiv.append(uploadWarning);
            //nrdsIdWarningDiv.insertAfter(nrdsWarning);
            nextButton.disabled = true;
          } else {
            targetDiv.find("span.maxSize1.requiredInfo").remove();
            //nrdsIdWarningDiv.find("div.maxSize1.requiredInfo").remove();
            nextButton.disabled = false;
          }
        } else {
          targetDiv.find("span.maxSize1.requiredInfo").remove();
          //nrdsIdWarningDiv.find("div.maxSize1.requiredInfo").remove();
          nextButton.disabled = false;
        }
      }
    );
    // check upload if contains no data, throw error message. 
    clickd_jquery("#f_upload_ab042de5ae80edfdcf8f9de2d905a1dc").on("change",
      function () {
        if (clickd_jquery("#f_7931698eeca3e9119c2700155d10128e")[0].value == 0 ||
        clickd_jquery("#f_d91f4f4ba61eeb119c5500155d10106c")[0].value == 0)
        if (this.files.length == 0)
        {
            targetDiv.append(uploadWarning);
            nextButton.disabled = true;
        } 
        else {
            targetDiv.find("span.maxSize1.requiredInfo").remove();
            nextButton.disabled = false;
          }
        }
    ); 
    }
});
