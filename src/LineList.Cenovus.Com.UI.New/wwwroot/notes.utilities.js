noteUtilities = {
    createNoteString: function (replaceMethod, existingNotes, newNotes) {
        if (replaceMethod == 'replace') {
            return newNotes;
        }
        else if (replaceMethod == 'add') {
            var noteList = noteUtilities.toNoteArray(existingNotes);
            var notesToAdd = newNotes.split(',');
            for (var noteToAdd in notesToAdd) {
                var noteVersionNumber = $.trim(notesToAdd[noteToAdd]);
                if ($.inArray(noteVersionNumber, noteList) == -1) {
                    noteList.push(noteVersionNumber);
                }
            }
            console.log(noteList.join(','));
            return noteList.join(',');
        }
        else if (replaceMethod == 'remove') {
            var noteList = noteUtilities.toNoteArray(existingNotes);
            var notesToRemove = newNotes.split(',');
            for (var noteToRemove in notesToRemove) {
                var noteVersionNumber = $.trim(notesToRemove[noteToRemove]);
                var position = $.inArray(noteVersionNumber, noteList);
                if (position !== -1) {
                    noteList.splice(position, 1);
                }
            }
            return noteList.join(',');
        }
    },

    toSimpleNotesString: function (noteString) {
        var noteList = noteUtilities.toNoteArray(noteString);
        var formattedNotes = new Array();
        for (var note in noteList) {
            var version = $.trim(noteList[note]);
            formattedNotes.push(noteUtilities.formatNote(version));
        }
        return formattedNotes.join(', ');
    },

    formatNote: function (value) {
        if (value === 'undefined') return '';
        var version = value.split('.');
        return !version ? '' : $.trim(version[0]);
    },

    findHighestNoteVersion: function (versionNumber, allNotes) {
        var maxVersion = versionNumber + '.0.0';
        for (var version in allNotes) {
            if (noteUtilities.isVersionLarger(maxVersion, version))
                maxVersion = version;
        }
        return maxVersion;
    },

    isVersionLarger: function (currentMax, version) {
        var maxParts = currentMax.split('.');
        var versionParts = version.split('.');

        if (maxParts[0] != versionParts[0]) return false;
        if (versionParts[1] < maxParts[1]) return false;
        if (versionParts[1] > maxParts[1]) return true;

        if (versionParts[2] > maxParts[2]) return true;

        return false;
    },

    toComplexNotesString: function (noteString, allNotes) {
        // lookup the latest version of the entered notes and put them back in the model
        var notes = noteUtilities.toNoteArray(noteString);
        var finalNoteList = new Array();

        for (var idx in notes) {
            var shortNoteString = $.trim(notes[idx]);
            if (shortNoteString !== "") {
                var noteVersion = noteUtilities.findHighestNoteVersion(shortNoteString, allNotes);
                finalNoteList.push(noteVersion);
            }
        }

        return finalNoteList.join(', ');
    },

    matches: function (simple, complex) {
        var toFind = noteUtilities.toNoteArray(simple);
        var toSearch = noteUtilities.toNoteArray(noteUtilities.toSimpleNotesString(complex));
        if (toFind.length == 0) return true;
        if (toFind.length == 0 && toSearch.length == 0) return true;

        var match = false;
        $.each(toFind, function (key, value) { match = ($.inArray(value, toSearch) > -1); return !match; });
        return match;
    },

    toNoteArray: function (value) {
        return value ? value.replace(/ /g, '').split(',') : [];
    }
}

/*
    Extend the string and array object with note helper methods
*/

String.prototype.noteArray = function () {
    return noteUtilities.toNoteArray(this);
}

String.prototype.complexNoteString = function (allNotes) {
    return noteUtilities.toComplexNotesString(this, allNotes);
}

String.prototype.simpleNoteString = function () {
    return noteUtilities.toSimpleNotesString(this);
}